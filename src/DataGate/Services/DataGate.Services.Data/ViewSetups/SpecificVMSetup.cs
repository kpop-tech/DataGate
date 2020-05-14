﻿namespace DataGate.Services.Data.ViewSetups
{
    using System;
    using System.Linq;

    using DataGate.Common;
    using DataGate.Services.Data.Funds.Contracts;
    using DataGate.Services.DateTime;
    using DataGate.Services.Mapping;
    using DataGate.Web.ViewModels.Entities;
    using DataGate.Web.ViewModels.Queries;

    public class SpecificVMSetup
    {
        private const int IndexStartConnectionInSQLTable = 0;
        private const int IndexEndConnectionInSQLTable = 1;

        public static T SetGet<T>(int id, string date, IFundDetailsService service)
        {
            service.ThrowEntityNotFoundExceptionIfIdDoesNotExist(id);

            var dateParsed = DateTimeParser.WebFormat(date);
            var entity = service.GetByIdAndDate(id, dateParsed);
            string startConnectionString = entity.ToList()[1][IndexStartConnectionInSQLTable];
            string endConnectionString = entity.ToList()[1][IndexEndConnectionInSQLTable];
            DateTime? endConnection = null;

            if (!string.IsNullOrWhiteSpace(endConnectionString) && endConnectionString != GlobalConstants.EmptyEndConnectionDisplay)
            {
                endConnection = DateTimeParser.SqlFormat(endConnectionString);
            }

            var distinctDocs = service.GetDistinctDocuments<DistinctDocViewModel>(id, dateParsed);
            var distinctAgrs = service.GetDistinctAgreements<DistinctDocViewModel>(id, dateParsed);

            var dto = new SpecificEntityOverviewGetDto()
            {
                Id = id,
                Date = date,
                Entity = entity,
                StartConnection = DateTimeParser.SqlFormat(startConnectionString),
                EndConnection = endConnection,
                DistinctDocuments = distinctDocs,
                DistinctAgreements = distinctAgrs,
            };

            return AutoMapperConfig.MapperInstance.Map<T>(dto);
        }
    }
}