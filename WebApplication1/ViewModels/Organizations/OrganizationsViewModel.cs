using OrganizationsWaterSupplyL4.Models;

namespace OrganizationsWaterSupplyL4.ViewModels.Organizations
{
    public class OrganizationsViewModel
    {
        public IEnumerable<Organization> Organizations { get; }
        public PageViewModel PageViewModel { get; }
        public OrganizationsFilterViewModel OrganizationsFilterViewModel { get; }
        public OrganizationsSortViewModel OrganizationsSortViewModel { get; }
        public OrganizationsViewModel(IEnumerable<Organization> organizations, PageViewModel viewModel, OrganizationsFilterViewModel filterModel, OrganizationsSortViewModel organizationsSortViewModel)
        {
            Organizations = organizations;
            PageViewModel = viewModel;
            OrganizationsFilterViewModel = filterModel;
            OrganizationsSortViewModel = organizationsSortViewModel;
        }
    }
}
