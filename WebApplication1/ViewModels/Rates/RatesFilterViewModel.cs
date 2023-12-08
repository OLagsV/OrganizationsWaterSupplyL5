using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using OrganizationsWaterSupplyL4.Models;
using System.ComponentModel.DataAnnotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace OrganizationsWaterSupplyL4.ViewModels.Rates
{
    public class RatesFilterViewModel
    {
        public RatesFilterViewModel(string rateName, decimal price)
        {
            SelectedRateName = rateName;
            SelectedPrice = price;
        }


        public string SelectedRateName { get; }
        public decimal SelectedPrice { get; }
    }
}
