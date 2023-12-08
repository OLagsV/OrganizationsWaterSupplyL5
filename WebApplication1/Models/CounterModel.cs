using MessagePack;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrganizationsWaterSupplyL4.Models
{
    public partial class CounterModel
    {
        public CounterModel()
        {
            Counters = new HashSet<Counter>();
        }
        [System.ComponentModel.DataAnnotations.Key]
        public int ModelId { get; set; }
        [Display(Name = "Марка")]
        [Required(ErrorMessage = "Не указано название марки")]
        [StringLength(50, MinimumLength = 3)]
        public string? ModelName { get; set; }
        [Display(Name = "Производитель")]
        [Required(ErrorMessage = "Не указан производитель")]
        [StringLength(50, MinimumLength = 3)]
        public string? Manufacturer { get; set; }
        [Display(Name = "Срок службы")]
        [Required(ErrorMessage = "Не указан срок службы")]
        [Range(1,100)]
        public int? ServiceTime { get; set; }

        public virtual ICollection<Counter> Counters { get; set; }
    }
}
