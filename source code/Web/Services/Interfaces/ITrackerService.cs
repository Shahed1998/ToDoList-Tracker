using Web.Models.Business_Entities;

namespace Web.Services.Interfaces
{
    public interface ITrackerService
    {
        Task<bool> AddTracker(TrackerViewModel viewModel);
        Task<(Pager<TrackerViewModel>, decimal?)> GetAllTrackers(int pageNumber = 1, int pageSize = 10);
        Task<bool> Delete(int Id);
    }
}
