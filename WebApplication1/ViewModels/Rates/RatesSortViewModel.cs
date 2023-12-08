using System.ComponentModel.DataAnnotations;

namespace OrganizationsWaterSupplyL4.ViewModels.Rates
{
    public enum SortState
    {
        RateNameAsc,
        RateNameDesc,  
        PriceAsc, 
        PriceDesc
    }
    public class RatesSortViewModel
    {
        public SortState RateNameSort { get; }
        public SortState PriceSort { get; }
        public SortState Current { get; }

        public RatesSortViewModel(SortState sortOrder)
        {
            RateNameSort = sortOrder == SortState.RateNameAsc ? SortState.RateNameDesc : SortState.RateNameAsc;
            PriceSort = sortOrder == SortState.PriceAsc ? SortState.PriceDesc : SortState.PriceAsc;
            Current = sortOrder;
        }
    }
}
