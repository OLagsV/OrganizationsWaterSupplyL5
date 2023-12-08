using OrganizationsWaterSupplyL4.Models;

namespace OrganizationsWaterSupplyL4.ViewModels.Rates
{
    public class RatesViewModel
    {
        public IEnumerable<Rate> Rates { get; }
        public PageViewModel PageViewModel { get; }
        public RatesFilterViewModel RatesFilterViewModel { get; }
        public RatesSortViewModel RatesSortViewModel { get; }
        public RatesViewModel(IEnumerable<Rate> rates, PageViewModel viewModel, RatesFilterViewModel ratesFilterViewModel, RatesSortViewModel ratesSortViewModel)
        {
            Rates = rates;
            PageViewModel = viewModel;
            RatesFilterViewModel = ratesFilterViewModel;
            RatesSortViewModel = ratesSortViewModel;
        }
    }
}
