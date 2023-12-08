using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrganizationsWaterSupplyL4.Models
{
    public partial class CountersDatum
    {
        [Key]
        public int CountersDataId { get; set; }
        [Display(Name = "Регистрационный номер")]
        public int? CounterRegistrationNumber { get; set; }
        [Display(Name = "Дата сбора показаний")]
        [Required(ErrorMessage = "Не указана дата сбора показаний")]
        [DataType(DataType.Date)]
        public DateTime? DataCheckDate { get; set; }
        [Display(Name = "Объем")]
        [Required(ErrorMessage = "Не указан объем")]
        [Range(0, 10000)]
        public int? Volume { get; set; }

        public virtual Counter? RegistrationNumber { get; set; }
    }
}
