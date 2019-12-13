﻿namespace Pharus.App.Controllers
{
    using System;
    using System.Linq;
    using System.Globalization;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.Rendering;

    using Pharus.Data;
    using Pharus.App.Utilities;
    using Pharus.Services.SubFunds.Contracts;
    using Pharus.App.Models.ViewModels.Entities;
    using Pharus.App.Models.BindingModels.SubFunds;
    using Microsoft.AspNetCore.Http;

    [Authorize]
    public class SubFundsController : Controller
    {
        private readonly Pharus_vFinale_Context context;
        private readonly ISubFundsService subFundsService;
        private readonly ISubFundsSelectListService subfundsSelectListService;
        private readonly ISubFundsFileService subFundsFileService;
        private readonly IHostingEnvironment hostingEnvironment;

        public SubFundsController(
            Pharus_vFinale_Context context,
            ISubFundsService subFundsService,
            ISubFundsSelectListService subfundsSelectListService,
            ISubFundsFileService subFundsFileService,
            IHostingEnvironment hostingEnvironment)
        {
            this.context = context;
            this.subFundsService = subFundsService;
            this.subfundsSelectListService = subfundsSelectListService;
            this.subFundsFileService = subFundsFileService;
            this.hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult All()
        {
            var model = new EntitiesViewModel
            {
                IsActive = true,
                ChosenDate = DateTime.Today.ToString("yyyy-MM-dd"),
            };

            GetAllActiveEntitiesUtility.GetAllActiveSubFundsWithHeaders(model, this.subFundsService);

            this.ModelState.Clear();
            return this.View(model);
        }

        public JsonResult AutoCompleteSubFundList(string searchTerm)
        {
            var result = this.context.TbHistorySubFund.ToList();
            if (searchTerm != null)
            {
                result = this.context.TbHistorySubFund.Where(s => s.SfOfficialSubFundName.Contains(searchTerm)).ToList();
            }

            var modifiedData = result.Select(s => new
            {
                id = s.SfOfficialSubFundName,
                text = s.SfOfficialSubFundName,
            });
            return this.Json(modifiedData);
        }

        [HttpPost]
        public IActionResult All(EntitiesViewModel model)
        {
            GetAllActiveEntitiesUtility.GetAllActiveSubFundsWithHeaders(model, this.subFundsService);

            var chosenDate = DateTime.ParseExact(model.ChosenDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            if (model.Command.Equals("Update Table"))
            {
                if (model.ChosenDate != null)
                {
                    if (model.IsActive)
                    {
                        GetAllActiveEntitiesUtility.GetAllActiveSubFundsWithHeaders(model, this.subFundsService);
                    }
                    else
                    {
                        model.Entities = this.subFundsService.GetAllSubFunds(chosenDate);
                    }
                }
            }
            else if (model.Command.Equals("Search"))
            {
                if (model.SearchTerm == null)
                {
                    return this.View(model);
                }

                model.Entities = new List<string[]>();

                var tableHeaders = this.subFundsService
                    .GetAllSubFunds()
                    .Take(1)
                    .ToList();

                List<string[]> tableWithoutHeaders = null;

                if (model.IsActive)
                {
                    tableWithoutHeaders = this.subFundsService
                        .GetAllSubFunds(chosenDate)
                        .Skip(1)
                        .Where(f => f.Contains("Active"))
                        .ToList();
                }
                else
                {
                    tableWithoutHeaders = this.subFundsService
                        .GetAllSubFunds()
                        .Skip(1)
                        .ToList();
                }

                CreateTableView.AddHeadersToView(model.Entities, tableHeaders);

                CreateTableView.AddTableToView(model.Entities, tableWithoutHeaders, model.SearchTerm.ToLower());
            }

            if (model.Entities != null)
            {
                return this.View(model);
            }

            return this.RedirectToPage("/SubFunds/All");
        }

        [HttpPost]
        public FileStreamResult ExtractExcelEntities(EntitiesViewModel model)
        {
            FileStreamResult fileStreamResult = null;

            string typeName = model.GetType().Name;
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            if (this.HttpContext.Request.Form.ContainsKey("extract_Excel"))
            {
                fileStreamResult = ExtractTable.ExtractTableAsExcel(model.Entities, typeName, controllerName);
            }

            return fileStreamResult;
        }

        [HttpPost]
        public FileStreamResult ExtractPdfEntities(EntitiesViewModel model)
        {
            FileStreamResult fileStreamResult = null;

            var chosenDate = DateTime.ParseExact(model.ChosenDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            string typeName = model.GetType().Name;
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            if (this.HttpContext.Request.Form.ContainsKey("extract_Pdf"))
            {
                fileStreamResult = ExtractTable.ExtractTableAsPdf(model.Entities, chosenDate, this.hostingEnvironment, typeName, controllerName);
            }

            return fileStreamResult;
        }

        [HttpGet("SubFunds/ViewEntitySE/{EntityId}/{ChosenDate}")]
        public IActionResult ViewEntitySE(int entityId, string chosenDate)
        {
            SpecificEntityViewModel viewModel = new SpecificEntityViewModel
            {
                ChosenDate = chosenDate,
                EntityId = entityId,
                Entity = this.subFundsService.GetSubFundById(entityId),
                EntitySubEntities = this.subFundsService.GetSubFund_ShareClasses(entityId),
                EntityTimeline = this.subFundsService.GetSubFundTimeline(entityId),
                EntityDocuments = this.subFundsService.GetAllSubFundDocumens(entityId),
                BaseEntityName = this.subFundsService.GetSubFund_FundContainer(entityId)[1][1],
            };

            this.ViewData["FileTypes"] = this.subfundsSelectListService.GetAllSubFundFileTypes();

            HttpContext.Session.SetString("entityId", Convert.ToString(entityId));

            string fileName = GetFileNameFromFilePath(entityId, chosenDate);

            if (string.IsNullOrEmpty(fileName))
            {
                return this.View(viewModel);
            }

            viewModel.FileNameToDisplay = fileName;

            this.ModelState.Clear();
            return this.View(viewModel);
        }

        private string GetFileNameFromFilePath(int entityId, string chosenDate)
        {
            return this.subFundsFileService.LoadSubFundFileToDisplay(entityId, chosenDate).Split('\\').Last();
        }

        public JsonResult AutoCompleteShareClassesList(string searchTerm, int entityId)
        {
            var entitiesToSearch = this.subFundsService.GetSubFund_ShareClasses(entityId).Skip(1).ToList();

            if (searchTerm != null)
            {
                entitiesToSearch = entitiesToSearch.Where(s => s[3].ToLower().Contains(searchTerm.ToLower())).ToList();
            }

            var modifiedData = entitiesToSearch.Select(s => new
            {
                id = s[3],
                text = s[3],
            });

            return this.Json(modifiedData);
        }

        [HttpPost("SubFunds/ViewEntitySE/{EntityId}")]
        public IActionResult ViewEntitySE(SpecificEntityViewModel viewModel)
        {
            viewModel.Entity = this.subFundsService.GetSubFundById(viewModel.EntityId);
            viewModel.EntitySubEntities = this.subFundsService.GetSubFund_ShareClasses(viewModel.EntityId);

            var chosenDate = DateTime.ParseExact(viewModel.ChosenDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            if (viewModel.Command.Equals("Update Table"))
            {
                if (viewModel.ChosenDate != null)
                {
                    viewModel.Entity = this.subFundsService.GetSubFundById(chosenDate, viewModel.EntityId);
                }
            }
            else if (viewModel.Command.Equals("Filter"))
            {
                if (viewModel.SearchTerm == null)
                {
                    return this.View(viewModel);
                }

                viewModel.EntitySubEntities = new List<string[]>();

                var tableHeaders = this.subFundsService.GetSubFund_ShareClasses(viewModel.EntityId).Take(1).ToList();
                var tableFundsWithoutHeaders = this.subFundsService.GetSubFund_ShareClasses(viewModel.EntityId).Skip(1).ToList();

                CreateTableView.AddHeadersToView(viewModel.EntitySubEntities, tableHeaders);

                CreateTableView.AddTableToView(viewModel.EntitySubEntities, tableFundsWithoutHeaders, viewModel.SearchTerm.ToLower());
            }

            if (viewModel.Entity != null && viewModel.EntitySubEntities != null)
            {
                return this.View(viewModel);
            }

            return this.View();
        }

        [HttpPost]
        public FileStreamResult ExtractExcelSubEntities(SpecificEntityViewModel model)
        {
            FileStreamResult fileStreamResult = null;

            string typeName = model.GetType().Name;
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            if (this.HttpContext.Request.Form.ContainsKey("extract_Excel"))
            {
                fileStreamResult = ExtractTable.ExtractTableAsExcel(model.EntitySubEntities, typeName, controllerName);
            }

            return fileStreamResult;
        }

        [HttpPost]
        public FileStreamResult ExtractPdfSubEntities(SpecificEntityViewModel model)
        {
            FileStreamResult fileStreamResult = null;

            var chosenDate = DateTime.ParseExact(model.ChosenDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            string typeName = model.GetType().Name;
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            if (this.HttpContext.Request.Form.ContainsKey("extract_Pdf"))
            {
                fileStreamResult = ExtractTable.ExtractTableAsPdf(model.EntitySubEntities, chosenDate, this.hostingEnvironment, typeName, controllerName);
            }

            return fileStreamResult;
        }

        [HttpGet("SubFunds/EditSubFund/{EntityId}")]
        public IActionResult EditSubFund(int entityId)
        {
            SubFundBindingModel model = new SubFundBindingModel
            {
                EntityProperties = this.subFundsService.GetSubFundWithDateById(entityId),
                CalculationDate = new SelectList(this.subfundsSelectListService.GetAllTbDomCalculationDate()),
                CesrClass = new SelectList(this.subfundsSelectListService.GetAllTbDomCesrClass()),
                CurrencyCode = new SelectList(this.subfundsSelectListService.GetAllTbDomCurrencyCode()),
                DerivMarket = new SelectList(this.subfundsSelectListService.GetAllTbDomDerivMarket()),
                DerivPurpose = new SelectList(this.subfundsSelectListService.GetAllTbDomDerivPurpose()),
                Frequency = new SelectList(this.subfundsSelectListService.GetAllTbDomFrequency()),
                GeographicalFocus = new SelectList(this.subfundsSelectListService.GetAllTbDomGeographicalFocus()),
                GlobalExposure = new SelectList(this.subfundsSelectListService.GetAllTbDomGlobalExposure()),
                PrincipalAssetClass = new SelectList(this.subfundsSelectListService.GetAllTbDomPrincipalAssetClass()),
                PrincipalInvestmentStrategy = new SelectList(this.subfundsSelectListService.GetAllTbDomPrincipalInvestmentStrategy()),
                SfCatBloomberg = new SelectList(this.subfundsSelectListService.GetAllTbDomSfCatBloomberg()),
                SfCatMorningStar = new SelectList(this.subfundsSelectListService.GetAllTbDomSfCatMorningStar()),
                SfCatSix = new SelectList(this.subfundsSelectListService.GetAllTbDomSfCatSix()),
                SfStatus = new SelectList(this.subfundsSelectListService.GetAllTbDomSFStatus()),
                TypeOfMarket = new SelectList(this.subfundsSelectListService.GetAllTbDomTypeOfMarket()),
                ValuationDate = new SelectList(this.subfundsSelectListService.GetAllTbDomValuationDate()),
            };

            return this.View(model);
        }

        [HttpPost]
        public IActionResult EditSubFund(SubFundBindingModel model)
        {
            // if (!ModelState.IsValid)
            // {
            //    return View(model ?? new EditFundBindingModel());
            // }
            int entityId = int.Parse(model.EntityProperties[1][0]);
            string returnUrl = $"/SubFunds/ViewSubFundSC/{entityId}";

            var subFund = this.subFundsService.GetSubFundById(entityId);

            if (this.HttpContext.Request.Form.ContainsKey("modify_button"))
            {
                for (int row = 1; row < subFund.Count; row++)
                {
                    for (int col = 0; col < subFund[row].Length; col++)
                    {
                        subFund[row][col] = model.EntityProperties[row][col];
                    }
                }

                return this.LocalRedirect(returnUrl);
            }

            return this.LocalRedirect(returnUrl);
        }

        [HttpGet]
        public IActionResult CreateSubFund()
        {
            SubFundBindingModel model = new SubFundBindingModel
            {
                EntityProperties = this.subFundsService.GetAllSubFunds(),
                CalculationDate = new SelectList(this.subfundsSelectListService.GetAllTbDomCalculationDate()),
                CesrClass = new SelectList(this.subfundsSelectListService.GetAllTbDomCesrClass()),
                CurrencyCode = new SelectList(this.subfundsSelectListService.GetAllTbDomCurrencyCode()),
                DerivMarket = new SelectList(this.subfundsSelectListService.GetAllTbDomDerivMarket()),
                DerivPurpose = new SelectList(this.subfundsSelectListService.GetAllTbDomDerivPurpose()),
                Frequency = new SelectList(this.subfundsSelectListService.GetAllTbDomFrequency()),
                GeographicalFocus = new SelectList(this.subfundsSelectListService.GetAllTbDomGeographicalFocus()),
                GlobalExposure = new SelectList(this.subfundsSelectListService.GetAllTbDomGlobalExposure()),
                PrincipalAssetClass = new SelectList(this.subfundsSelectListService.GetAllTbDomPrincipalAssetClass()),
                PrincipalInvestmentStrategy = new SelectList(this.subfundsSelectListService.GetAllTbDomPrincipalInvestmentStrategy()),
                SfCatBloomberg = new SelectList(this.subfundsSelectListService.GetAllTbDomSfCatBloomberg()),
                SfCatMorningStar = new SelectList(this.subfundsSelectListService.GetAllTbDomSfCatMorningStar()),
                SfCatSix = new SelectList(this.subfundsSelectListService.GetAllTbDomSfCatSix()),
                SfStatus = new SelectList(this.subfundsSelectListService.GetAllTbDomSFStatus()),
                TypeOfMarket = new SelectList(this.subfundsSelectListService.GetAllTbDomTypeOfMarket()),
                ValuationDate = new SelectList(this.subfundsSelectListService.GetAllTbDomValuationDate()),
            };

            return this.View(model);
        }
    }
}