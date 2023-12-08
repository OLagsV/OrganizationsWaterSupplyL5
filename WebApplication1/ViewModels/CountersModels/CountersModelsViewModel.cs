using OrganizationsWaterSupplyL4.Models;
using OrganizationsWaterSupplyL4.ViewModels.Counters;

namespace OrganizationsWaterSupplyL4.ViewModels.CountersModels
{
    public class CountersModelsViewModel
    {
        public IEnumerable<CounterModel> CounterModels { get; }
        public PageViewModel PageViewModel { get; }
        public CountersModelsFilterViewModel CountersModelsFilterViewModel { get; }
        public CountersModelsSortViewModel CountersModelsSortViewModel { get; }
        public CountersModelsViewModel(IEnumerable<CounterModel> models, PageViewModel viewModel, CountersModelsFilterViewModel countersModelsFilterViewModel, CountersModelsSortViewModel countersModelsSortViewModel)
        {
            CounterModels = models;
            PageViewModel = viewModel;
            CountersModelsFilterViewModel = countersModelsFilterViewModel;
            CountersModelsSortViewModel = countersModelsSortViewModel;
        }
    }
}
