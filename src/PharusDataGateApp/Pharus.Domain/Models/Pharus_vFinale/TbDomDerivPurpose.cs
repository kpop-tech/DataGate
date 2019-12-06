﻿namespace Pharus.Domain.Models.Pharus_vFinale
{
    using System.Collections.Generic;

    public partial class TbDomDerivPurpose
    {
        public TbDomDerivPurpose()
        {
            TbHistorySubFund = new HashSet<TbHistorySubFund>();
        }

        public int DpId { get; set; }

        public string DpDesc { get; set; }

        public virtual ICollection<TbHistorySubFund> TbHistorySubFund { get; set; }
    }
}