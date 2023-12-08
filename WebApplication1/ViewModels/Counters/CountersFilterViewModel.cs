using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using OrganizationsWaterSupplyL4.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace OrganizationsWaterSupplyL4.ViewModels.Counters
{
    public class CountersFilterViewModel
    {
        public CountersFilterViewModel(List<CounterModel> models, List<Organization> organizations, int modelId, int organizationId, DateTime timeOfInstallation)
        {
            models.Insert(0, new CounterModel
            {
                ModelId = 0,
                ModelName = "Все",
                Manufacturer = "Manufacturer",
                ServiceTime = 0,
            });
            Models = new SelectList(models, "ModelId", "ModelName", modelId);
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
            SelectedModel = modelId;
            SelectedOrganization = organizationId;
            SelectedTimeOfInstallation = timeOfInstallation;
        }

        public SelectList Models { get; }
        public SelectList Organizations { get; }
        public int SelectedModel { get; }
        public int SelectedOrganization { get; }
        public DateTime SelectedTimeOfInstallation { get; }
    }
}
