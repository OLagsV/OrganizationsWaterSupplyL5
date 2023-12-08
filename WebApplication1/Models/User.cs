using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace OrganizationsWaterSupplyL4.Models
{
    public class User : IdentityUser
    {
        [Display(Name = "Дата регистрации")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime RegistrationDate { get; set; }
    }
}