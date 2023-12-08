using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using OrganizationsWaterSupplyL4.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace OrganizationsWaterSupplyL4.ViewModels.CountersData
{
    public class CountersDataFilterViewModel
    {
        public CountersDataFilterViewModel(List<Counter> counters, DateTime dataCheckDate, int volume, int registartionNumber)
        {
            counters.Insert(0, new Counter
            {
                RegistrationNumber = 0,
                ModelId = 0,
                TimeOfInstallation = new DateTime(),
                OrganizationId = 0
            });
            Counters = new SelectList(counters, "RegistrationNumber", "RegistrationNumber", registartionNumber);
            SelectedCounterRegistrationNumber = registartionNumber;
            SelectedVolume = volume;
            SelectedDataCheckDate = dataCheckDate;
        }

        public SelectList Counters { get; }
        public int SelectedCounterRegistrationNumber { get; }
        public int SelectedVolume { get; }
        public DateTime SelectedDataCheckDate { get; }
    }
}
