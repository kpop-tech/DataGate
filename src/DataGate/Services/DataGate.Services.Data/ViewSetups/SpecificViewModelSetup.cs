namespace DataGate.Services.Data.ViewSetups
{
    using System.Linq;

    using DataGate.Services.DateTime;
    using DataGate.Web.ViewModels.Entities;

    public class SpecificViewModelSetup
    {
        private const int IndexStartConnectionInSQLTable = 0;
        private const int IndexEndConnectionInSQLTable = 1;

        public static void PrepareModel(SpecificEntityViewModel model, IEntitySubEntitiesService<string[]> service)
        {
            service.ThrowEntityNotFoundExceptionIfIdDoesNotExist(model.Id);

            var date = DateTimeParser.WebFormat(model.Date);
            int entityId = model.Id;

            model.Entity = service.GetEntityWithDateById(date, entityId).ToList();
            model.EntityDistinctDocuments = service.GetDistinctDocuments(date, entityId).ToList();
            model.EntityDistinctAgreements = service.GetDistinctAgreements(date, entityId).ToList();

            model.EntitySubEntities = service.GetEntity_SubEntities(date, entityId).ToList();
            model.Headers = service
                                                            .GetEntity_SubEntities(date, entityId)
                                                            .Take(1)
                                                            .ToList();
            model.EntityTimeline = service.GetTimeline(entityId).ToList();
            model.EntityDocuments = service.GetAllDocuments(entityId).ToList();
            model.EntityAgreements = service.GetAllAgreements(date, entityId).ToList();

            string startConnection = model.Entity.ToList()[1][IndexStartConnectionInSQLTable];
            model.StartConnection = DateTimeParser.SqlFormat(startConnection);

            if (model.EndConnection != null)
            {
                string endConnection = model.Entity.ToList()[1][IndexEndConnectionInSQLTable];
                model.EndConnection = DateTimeParser.SqlFormat(endConnection);
            }

            //this.ViewData["DocumentFileTypes"] = this.fundsSelectListService.GetAllProspectusFileTypes();
            //this.ViewData["AgreementsFileTypes"] = this.fundsSelectListService.GetAllAgreementsFileTypes();
            //this.ViewData["AgreementsStatus"] = this.agreementsSelectListService.GetAllTbDomAgreementStatus();
            //this.ViewData["Companies"] = this.agreementsSelectListService.GetAllTbCompanies();
        }

        //private void CallEntitySubEntitiesWithSelectedColumns(SpecificEntityViewModel model, DateTime chosenDate)
        //{
        //    model.EntitySubEntities = this.service
        //        .GetFund_SubFundsWithSelectedViewAndDate(
        //                                        model.PreSelectedColumns,
        //                                        model.SelectedColumns,
        //                                        chosenDate,
        //                                        model.EntityId)
        //        .ToList();
        //}
    }
}
