﻿using System;
using System.Collections.Generic;

namespace Pharus.Models.Pharus_vFinale
{
    public partial class TbServiceAgreementShareclass
    {
        public int SaId { get; set; }
        public int SaSc { get; set; }
        public int SaActivityType { get; set; }
        public DateTime SaConctractDate { get; set; }
        public DateTime? SaActivationDate { get; set; }
        public DateTime? SaExpirationDate { get; set; }
        public string SaRifCode { get; set; }
        public int? SaCompanyId { get; set; }
        public string SaStatus { get; set; }

        public virtual TbDomActivityType SaActivityTypeNavigation { get; set; }
        public virtual TbCompanies SaCompany { get; set; }
        public virtual TbShareClass SaScNavigation { get; set; }
    }
}
