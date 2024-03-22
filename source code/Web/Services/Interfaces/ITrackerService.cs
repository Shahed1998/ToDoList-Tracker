using Web.Models.Business_Entities;

namespace Web.Services.Interfaces
{
    public interface ITrackerService
    {
        Task<bool> AddTracker(TrackerViewModel viewModel);
        Task<(IEnumerable<TrackerViewModel>, decimal?)> GetAllTrackers();
        Task<bool> Delete(int Id);
    }
}
