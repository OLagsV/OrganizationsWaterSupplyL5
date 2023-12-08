using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using OrganizationsWaterSupplyL4.Models;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace OrganizationsWaterSupplyL4.ViewModels.Organizations
{
    public class OrganizationsFilterViewModel
    {
        public OrganizationsFilterViewModel(string orgName, string ownershipType)
        {
            SelectedOrgName = orgName;
            SelectedOwnType = ownershipType;
        }

        public string SelectedOrgName { get; }
        public string SelectedOwnType { get; }
    }
}
