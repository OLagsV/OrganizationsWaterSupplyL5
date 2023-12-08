using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using OrganizationsWaterSupplyL4.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace OrganizationsWaterSupplyL4.ViewModels.RateOrgNotes
{
    public class RateOrgNotesFilterViewModel
    {
        public RateOrgNotesFilterViewModel(List<Rate> rates, List<Organization> organizations, int rateId, int organizationId)
        {
            rates.Insert(0, new Rate
            {
                RateId = 0,
                RateName = "Все",
                Price = 0,
            });
            Rates = new SelectList(rates, "RateId", "RateName", rateId);
            organizations.Insert(0, new Organization
            {
                OrganizationId = 0,
                OrgName = "Все",
                OwnershipType = "type",
                Adress = "Adress",
                DirectorFullname = "Director",
                DirectorPhone = "DPhone",
                ResponsibleFullname = "Responsible",
                ResponsiblePhone = "RPhone"
            });
            Organizations = new SelectList(organizations, "OrganizationId", "OrgName", organizationId);
            SelectedRate = rateId;
            SelectedOrganization = organizationId;
        }

        public SelectList Rates { get; }
        public SelectList Organizations { get; }
        public int SelectedRate { get; }
        public int SelectedOrganization { get; }
    }
}
