﻿namespace DataGate.Common
{
    public static class GlobalConstants
    {
        // Application Constants
        public const string SystemName = "DataGate";
        public const string DataGateFooterDisplay = "- Pharus Management Lux SA 16, Avenue de la Gare L- 1610 Luxembourg T. +352 28 55 69 - 1";
        public const int MaxAdminCount = 3;

        // Controllers, areas
        public const string ControllerRouteDataValue = "controller";
        public const string FundsAreaName = "Funds";
        public const string FundsControllerName = "Funds";
        public const string FundDetailsControllerName = "FundDetails";
        public const string FundSubEntitiesControllerName = "FundSubEntities";
        public const string SubFundsControllerName = "SubFunds";
        public const string SubFundDetailsControllerName = "SubFundDetails";
        public const string SubFundSubEntitiesControllerName = "SubFundSubEntities";
        public const string ShareClassesControllerName = "ShareClasses";
        public const string ShareClassesSubEntitiesControllerName = "ShareClassesSubEntities";

        // Routes names
        public const string AllFundsRouteName = "allFunds";
        public const string FundSubEntititesRouteName = "fundSubEntities";
        public const string AllSubFundsRouteName = "allSubFunds";
        public const string SubFundSubEntititesRouteName = "subFundSubEntities";
        public const string AllShareClassesRouteName = "allShareClasses";
        public const string ShareClassSubEntititesRouteName = "shareClassSubEntities";

        // Tempdata Keys
        public const string InfoKey = "info";
        public const string ParentKey = "parent";
        public const string ErrorKey = "error";

        // Connection Strings
        public const string DataGateUsersConnection = "DataGateUsersConnection";
        public const string DataGatevFinaleConnection = "DataGate_vFinaleConnection";

        // User Roles
        public const string AdministratorRoleName = "Admin";
        public const string RiskRoleName = "Risk";
        public const string LegalRoleName = "Legal";
        public const string InvestmentRoleName = "Investement";
        public const string ComplianceRoleName = "Compliance";
        public const string GuestRoleName = "Guest";

        // Email
        public const string ConfirmEmailSubject = "Confirm your email";
        public const string EmailConfirmationMessage = "Please confirm your account by <a href='{0}'>clicking here</a>.";
        public const string ResetPasswordEmailSubject = "Reset Password";
        public const string PasswordResetMessage = "Please reset your password by <a href='{0}'>clicking here</a>.";

        // SQL Queries
        public const string SqlItemFormatRequired = "[{0}]";

        // Urls, actions
        public const string FundAllUrl = "/f/all";
        public const string ByIdAndDateActionName = "ByIdAndDate";

        // Dates
        public const string LastLoginTimeDisplay = "dd.MM.yyyy HH:mm";
        public const string RequiredWebDateTimeFormat = "yyyy-MM-dd";
        public const string RequiredSqlDateTimeFormat = "yyyyMMdd";
        public const string SqlDateTimeFormatParsing = "dd/MM/yyyy";
        public const string PdfDateTimeFormatDisplay = "dd MMMM yyyy";
        public const string EmptyEndConnectionDisplay = "STILL VALID";

        // Folder names, mime types, formats, file names
        public const string ExcelFileExtension = ".xlsx";
        public const string PdfFileExtension = ".pdf";
        public const string ExcelStreamMimeType = "application/excel";
        public const string PdfStreamMimeType = "application/pdf";
        //public static string[] AllowedFileExtensoins = new string[] { ".ppt", ".pptx", ".doc", ".docx", ".xls", ".cs", ".zip", ".json", ".xml", ".mp4", ".avi", ".txt", ".html", ".pdf" };

        // Entities
        public const int IndexEntityIdInTable = 2;
        public const int IndexEntityNameInTable = 3;
        public const int IndexEntityHeadersInTable = 0;
        public const int RowNumberOfHeadersInTable = 1;
        public const int NumberOfAllowedColumnsInPdfView = 16;
        public const string DefaultAutocompleteSelectTerm = "Quick Select";
        public const string ContainerFundName = "FUND";
        public const string ContainerSubFundName = "SUB FUND";
        public const string UploadTypeProspectusDisplay = "Prospectus";
        public const string UploadTypeDocumentDisplay = "Document";
        public const string FundNameDisplay = "Fund";
        public const string SubFundNameDisplay = "Sub Fund";
        public const string ShareClassNameDisplay = "Share Class";

        // Toolbar Commands
        public const string CommandUpdateTable = "Update Table";
        public const string CommandExtractExcel = "Excel";
        public const string CommandExtractPdf = "PDF";
    }
}
