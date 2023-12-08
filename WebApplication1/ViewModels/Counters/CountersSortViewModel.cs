namespace OrganizationsWaterSupplyL4.ViewModels.Counters
{
    public enum SortState
    {
        TimeOfInstallationAsc,
        TimeOfInstallationDesc,  
        ModelAsc, 
        ModelDesc, 
        OrganizationAsc,
        OrganizationDesc
    }
    public class CountersSortViewModel
    {
        public SortState TimeOfInstallationSort { get; }
        public SortState ModelSort { get; }
        public SortState OrganizationSort { get; }
        public SortState Current { get; }
        public CountersSortViewModel(SortState sortOrder)
        {
            TimeOfInstallationSort = sortOrder == SortState.TimeOfInstallationAsc ? SortState.TimeOfInstallationDesc : SortState.TimeOfInstallationAsc;
            ModelSort = sortOrder == SortState.ModelAsc ? SortState.ModelDesc : SortState.ModelAsc;
            OrganizationSort = sortOrder == SortState.OrganizationAsc ? SortState.OrganizationDesc : SortState.OrganizationAsc;
            Current = sortOrder;
        }
    }
}
