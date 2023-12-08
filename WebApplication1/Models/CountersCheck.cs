using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrganizationsWaterSupplyL4.Models
{
    public partial class CountersCheck
    {
        [Key]
        public int CountersCheckId { get; set; }
        [Display(Name = "Регистрационный номер")]
        public int? CounterRegistrationNumber { get; set; }
        [Display(Name = "Дата поверки")]
        [Required(ErrorMessage = "Не указана дата поверки")]
        [DataType(DataType.Date)]
        public DateTime? CheckDate { get; set; }
        [Display(Name = "Результат поверки")]
        [Required(ErrorMessage = "Не указан результат поверки")]
        [StringLength(50, MinimumLength = 1)]
        public string? CheckResult { get; set; }

        public virtual Counter? RegistrationNumber { get; set; }
    }
}
