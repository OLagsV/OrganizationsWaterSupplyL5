using OrganizationsWaterSupplyL4.Models;

namespace OrganizationsWaterSupplyL4.ViewModels.RateOrgNotes
{
    public class RateOrgNotesViewModel
    {
        public IEnumerable<RateOrgNote> RateOrgNotes { get; }
        public PageViewModel PageViewModel { get; }
        public RateOrgNotesFilterViewModel RateOrgNotesFilterViewModel { get; }
        public RateOrgNotesSortViewModel RateOrgNotesSortViewModel { get; }
        public RateOrgNotesViewModel(IEnumerable<RateOrgNote> notes, PageViewModel viewModel, RateOrgNotesFilterViewModel rateOrgNotesFilter, RateOrgNotesSortViewModel rateOrgNotesSort)
        {
            RateOrgNotes = notes;
            PageViewModel = viewModel;
            RateOrgNotesFilterViewModel = rateOrgNotesFilter;
            RateOrgNotesSortViewModel = rateOrgNotesSort;
        }
    }
}
