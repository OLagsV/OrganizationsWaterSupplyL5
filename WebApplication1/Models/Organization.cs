using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrganizationsWaterSupplyL4.Models
{
    public partial class Organization
    {
        public Organization()
        {
            Counters = new HashSet<Counter>();
            RateOrgNotes = new HashSet<RateOrgNote>();
        }

        public int OrganizationId { get; set; }
        [Display(Name = "Название организации")]
        [Required(ErrorMessage = "Не указано название организации")]
        [StringLength(50, MinimumLength = 3)]
        public string? OrgName { get; set; }
        [Display(Name = "Тип собственности")]
        [Required(ErrorMessage = "Не указан тип собственности")]
        [StringLength(50, MinimumLength = 3)]
        public string? OwnershipType { get; set; }
        [Display(Name = "Адресс")]
        [Required(ErrorMessage = "Не указан адресс")]
        [StringLength(50, MinimumLength = 3)]
        public string? Adress { get; set; }
        [Display(Name = "ФИО директора")]
        [Required(ErrorMessage = "Не указано ФИО директора")]
        [StringLength(50, MinimumLength = 3)]
        public string? DirectorFullname { get; set; }
        [Display(Name = "Телефон директора")]
        [Required(ErrorMessage = "Не указан номер директора")]
        [StringLength(50, MinimumLength = 3)]
        public string? DirectorPhone { get; set; }
        [Display(Name = "ФИО представителя")]
        [Required(ErrorMessage = "Не указано ФИО ответственного сотрудника")]
        [StringLength(50, MinimumLength = 3)]
        public string? ResponsibleFullname { get; set; }
        [Display(Name = "Телефон представителя")]
        [Required(ErrorMessage = "Не указан номер ответственного сотрудника")]
        [StringLength(50, MinimumLength = 3)]
        public string? ResponsiblePhone { get; set; }

        public virtual ICollection<Counter> Counters { get; set; }
        [NotMapped]
        public virtual ICollection<RateOrgNote> RateOrgNotes { get; set; }
    }
}
