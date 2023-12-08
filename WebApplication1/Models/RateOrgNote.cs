using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrganizationsWaterSupplyL4.Models
{
    public partial class RateOrgNote
    {
        public int RateOrgNoteId { get; set; }
        [Display(Name = "Тариф")]
        public int? RateId { get; set; }
        [Display(Name = "Организация")]
        public int? OrganizationId { get; set; }

        public virtual Organization? Organization { get; set; }
        public virtual Rate? Rate { get; set; }
    }
}
