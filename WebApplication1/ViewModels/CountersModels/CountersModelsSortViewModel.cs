using System.ComponentModel.DataAnnotations;

namespace OrganizationsWaterSupplyL4.ViewModels.CountersModels
{
    public enum SortState
    {
        ModelNameAsc,
        ModelNameDesc,  
        ManufacturerAsc,
        ManufacturerDesc, 
        ServiceTimeAsc,
        ServiceTimeDesc,
    }
    public class CountersModelsSortViewModel
    {
        public SortState ModelNameSort { get; }
        public SortState ManufacturerSort { get; }
        public SortState ServiceTimeSort { get; }
        public SortState Current { get; }

        public CountersModelsSortViewModel(SortState sortOrder)
        {
            ModelNameSort = sortOrder == SortState.ModelNameAsc ? SortState.ModelNameDesc : SortState.ModelNameAsc;
            ManufacturerSort = sortOrder == SortState.ManufacturerAsc ? SortState.ManufacturerDesc : SortState.ManufacturerAsc;
            ServiceTimeSort = sortOrder == SortState.ServiceTimeAsc ? SortState.ServiceTimeDesc : SortState.ServiceTimeAsc;
            Current = sortOrder;
        }
    }
}
