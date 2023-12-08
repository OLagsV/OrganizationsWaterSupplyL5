using System.ComponentModel.DataAnnotations;

namespace OrganizationsWaterSupplyL4.ViewModels.CountersData
{
    public enum SortState
    {
        DataCheckDateAsc,
        DataCheckDateDesc,  
        VolumeAsc, 
        VolumeDesc
    }
    public class CountersDataSortViewModel
    {
        public SortState DataCheckDateSort { get; }
        public SortState VolumeSort { get; }
        public SortState Current { get; }
        public CountersDataSortViewModel(SortState sortOrder)
        {
            DataCheckDateSort = sortOrder == SortState.DataCheckDateAsc ? SortState.DataCheckDateDesc : SortState.DataCheckDateAsc;
            VolumeSort = sortOrder == SortState.VolumeAsc ? SortState.VolumeDesc : SortState.VolumeAsc;
            Current = sortOrder;
        }
    }
}
