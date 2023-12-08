using OrganizationsWaterSupplyL4.Models;

namespace OrganizationsWaterSupplyL4.ViewModels.CountersChecks
{
    public class CountersChecksViewModel
    {
        public IEnumerable<CountersCheck> CountersChecks { get; }
        public PageViewModel PageViewModel { get; }
        public CountersChecksFilterViewModel CountersChecksFilterViewModel { get; }
        public CountersChecksSortViewModel CountersChecksSortViewModel { get; }
        public CountersChecksViewModel(IEnumerable<CountersCheck> checks, PageViewModel viewModel, CountersChecksFilterViewModel countersChecksFilter, CountersChecksSortViewModel countersChecksSort)
        {
            CountersChecks = checks;
            PageViewModel = viewModel;
            CountersChecksFilterViewModel = countersChecksFilter;
            CountersChecksSortViewModel = countersChecksSort;
        }
    }
}
