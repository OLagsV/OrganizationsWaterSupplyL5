using OrganizationsWaterSupplyL4.Models;

namespace OrganizationsWaterSupplyL4.ViewModels.Counters
{
    public class CountersViewModel
    {
        public IEnumerable<Counter> Counters { get; }
        public PageViewModel PageViewModel { get; }
        public CountersFilterViewModel CountersFilterViewModel { get; }
        public CountersSortViewModel CountersSortViewModel { get; }
        public CountersViewModel(IEnumerable<Counter> counters, PageViewModel viewModel, CountersFilterViewModel countersFilterViewModel, CountersSortViewModel countersSortViewModel)
        {
            Counters = counters;
            PageViewModel = viewModel;
            CountersFilterViewModel = countersFilterViewModel;
            CountersSortViewModel = countersSortViewModel;
        }
    }
}
