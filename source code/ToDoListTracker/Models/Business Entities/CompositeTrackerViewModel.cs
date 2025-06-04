using ToDoListTracker.Models.Business_Entities;

namespace Web.Models.Business_Entities
{
    public class CompositeTrackerViewModel
    {
        public TrackerViewModel? TrackerViewModel { get; set; }
        public Pager<TrackerViewModel>? Pager { get; set; }
        public IndexVM IndexVM { get; set; } = new IndexVM();
        public decimal Achievements { get; set; } = decimal.Zero;
        public string Title { get; set; } = string.Empty;
    }
}
