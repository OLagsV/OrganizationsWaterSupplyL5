using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using OrganizationsWaterSupplyL4.Models;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace OrganizationsWaterSupplyL4.ViewModels.CountersModels
{
    public class CountersModelsFilterViewModel
    {
        public CountersModelsFilterViewModel(string modelName, string manufacturer, int serviceTime)
        {
            SelectedModelName = modelName;
            SelectedManufacturer = manufacturer;
            SelectedServiceTime = serviceTime;
        }

        public SelectList Posts { get; }

        public string SelectedModelName { get; }
        public string SelectedManufacturer { get; }
        public int SelectedServiceTime { get; }
    }
}
