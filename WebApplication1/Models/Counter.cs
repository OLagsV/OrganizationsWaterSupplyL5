using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrganizationsWaterSupplyL4.Models
{
    public partial class Counter
    {
        public Counter()
        {
            CountersChecks = new HashSet<CountersCheck>();
            CountersData = new HashSet<CountersDatum>();
        }
        [Key]
        [Display(Name = "Регистрационный номер")]
        public int RegistrationNumber { get; set; }
        [Display(Name = "Марка")]
        [Required(ErrorMessage = "Не указана марка счетчика")]
        public int? ModelId { get; set; }
        [Display(Name = "Время установки")]
        [Required(ErrorMessage = "Не указана дата установки")]
        [DataType(DataType.Date)]
        public DateTime? TimeOfInstallation { get; set; }
        [Display(Name = "Орагнизация")]
        [Required(ErrorMessage = "Не указана организация")]
        public int? OrganizationId { get; set; }

        public virtual CounterModel? Model { get; set; }
        public virtual Organization? Organization { get; set; }
        public virtual ICollection<CountersCheck> CountersChecks { get; set; }
        public virtual ICollection<CountersDatum> CountersData { get; set; }
    }
}
