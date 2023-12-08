namespace OrganizationsWaterSupplyL4.ViewModels.RateOrgNotes
{
    public enum SortState
    {
        RateAsc, 
        RateDesc, 
        OrganizationAsc,
        OrganizationDesc
    }
    public class RateOrgNotesSortViewModel
    {
        public SortState RateSort { get; }
        public SortState OrganizationSort { get; }
        public SortState Current { get; }
        public RateOrgNotesSortViewModel(SortState sortOrder)
        {
            RateSort = sortOrder == SortState.RateAsc ? SortState.RateDesc : SortState.RateAsc;
            OrganizationSort = sortOrder == SortState.OrganizationAsc ? SortState.OrganizationDesc : SortState.OrganizationAsc;
            Current = sortOrder;
        }
    }
}
