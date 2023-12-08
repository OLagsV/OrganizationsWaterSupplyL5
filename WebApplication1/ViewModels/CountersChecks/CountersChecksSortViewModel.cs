using System.ComponentModel.DataAnnotations;

namespace OrganizationsWaterSupplyL4.ViewModels.CountersChecks
{
    public enum SortState
    {
        CheckDateAsc,
        CheckDateDesc,  
        ResultAsc, 
        ResultDesc
    }
    public class CountersChecksSortViewModel
    {
        public SortState CheckDateSort { get; }
        public SortState ResultSort { get; }
        public SortState Current { get; }
        public CountersChecksSortViewModel(SortState sortOrder)
        {
            CheckDateSort = sortOrder == SortState.CheckDateAsc ? SortState.CheckDateDesc : SortState.CheckDateAsc;
            ResultSort = sortOrder == SortState.ResultAsc ? SortState.ResultDesc : SortState.ResultAsc;
            Current = sortOrder;
        }
    }
}
