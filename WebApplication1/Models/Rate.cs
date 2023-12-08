using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrganizationsWaterSupplyL4.Models
{
    public partial class Rate
    {
        public int RateId { get; set; }
        [Display(Name = "Название тарифа")]
        [Required(ErrorMessage = "Не указано название тарифа")]
        [StringLength(50, MinimumLength = 3)]
        public string? RateName { get; set; }
        [Display(Name = "Цена")]
        [Required(ErrorMessage = "Не указана цена тарифа")]
        [Range(0,10000)]
        public decimal? Price { get; set; }
        [NotMapped]
        public virtual RateOrgNote? RateOrgNote { get; set; }
    }
}
