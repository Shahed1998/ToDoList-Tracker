namespace Web.Models.Business_Entities
{
    public class CompositeTrackerViewModel
    {
        public TrackerViewModel? TrackerViewModel { get; set; }
        public Pager<TrackerViewModel>? Pager { get; set; }  
    }
}
