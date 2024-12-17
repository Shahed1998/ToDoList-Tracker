using Web.Models.Business_Entities;

namespace Web.Services.Interfaces
{
    public interface ITrackerService
    {
        Task<bool> AddTracker(TrackerViewModel viewModel);
        Task<(Pager<TrackerViewModel>, decimal?)> GetAllTrackers(int pageNumber = 1, int pageSize = 10, string? userId = null);
        Task<TrackerViewModel?> GetTrackerById(int id);
        Task<bool> Delete(int Id);
        Task<bool> Update(TrackerViewModel viewModel);
        Task<bool> DeleteAll(string? userId=null);
    }
}
