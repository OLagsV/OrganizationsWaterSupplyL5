using OrganizationsWaterSupplyL4.Models;

namespace OrganizationsWaterSupplyL4.ViewModels.CountersData
{
    public class CountersDataViewModel
    {
        public IEnumerable<CountersDatum> CounterData { get; }
        public PageViewModel PageViewModel { get; }
        public CountersDataFilterViewModel CountersDataFilterViewModel { get; }
        public CountersDataSortViewModel CountersDataSortViewModel { get; }
        public CountersDataViewModel(IEnumerable<CountersDatum> data, PageViewModel viewModel, CountersDataFilterViewModel countersDataFilterViewModel, CountersDataSortViewModel countersDataSortViewModel)
        {
            CounterData = data;
            PageViewModel = viewModel;
            CountersDataFilterViewModel = countersDataFilterViewModel;
            CountersDataSortViewModel = countersDataSortViewModel;
        }
    }
}
