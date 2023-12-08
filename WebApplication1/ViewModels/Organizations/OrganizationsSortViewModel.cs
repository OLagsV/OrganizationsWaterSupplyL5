namespace OrganizationsWaterSupplyL4.ViewModels.Organizations
{
    public enum SortState
    {
        OrgNameAsc,
        OrgNameDesc,  
        OwnerTypeAsc, 
        OwnerTypeDesc, 
        AdressAsc,
        AdressDesc,
        DirectorAsc,
        DirectorDesc,
        ResponsibleAsc,
        ResponsibleDesc
    }
    public class OrganizationsSortViewModel
    {
        public SortState OrgNameSort { get; }
        public SortState OwnerTypeSort { get; }
        public SortState AdressSort { get; }
        public SortState DirectorSort { get; }
        public SortState ResponsibleSort { get; }
        public SortState Current { get; }

        public OrganizationsSortViewModel(SortState sortOrder)
        {
            OrgNameSort = sortOrder == SortState.OrgNameAsc ? SortState.OrgNameDesc : SortState.OrgNameAsc;
            OwnerTypeSort = sortOrder == SortState.OwnerTypeAsc ? SortState.OwnerTypeDesc : SortState.OwnerTypeAsc;
            AdressSort = sortOrder == SortState.AdressAsc ? SortState.AdressDesc : SortState.AdressAsc;
            DirectorSort = sortOrder == SortState.DirectorAsc ? SortState.DirectorDesc : SortState.DirectorAsc;
            ResponsibleSort = sortOrder == SortState.ResponsibleAsc ? SortState.ResponsibleDesc : SortState.ResponsibleAsc;
            Current = sortOrder;
        }
    }
}
