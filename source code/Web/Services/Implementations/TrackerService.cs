using Web.Models.Business_Entities;
using Web.Models.General_Entities;
using Web.Repositories.Interfaces;
using Web.Services.Interfaces;

namespace Web.Services.Implementations
{
    public class TrackerService : ITrackerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrackerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddTracker(TrackerViewModel viewModel)
        {
            return await _unitOfWork.TrackerRepository.Add((Tracker)viewModel);
        }

        public async Task<(Pager<TrackerViewModel>, decimal?)> GetAllTrackers(int pageNumber = 1, int pageSize = 10, string? userId = null)
        {
            var result = await _unitOfWork.TrackerRepository.GetAll(pageNumber, pageSize, userId);

            var lst = result.Item1.Select(tracker => (TrackerViewModel)tracker).ToList();

            var test = new Pager<TrackerViewModel>(lst, result.Item1.TotalCount, pageNumber, pageSize);

            return (test, result.Item2);
        }

        public async Task<bool> Delete(int Id)
        {
            return await _unitOfWork.TrackerRepository.Delete(Id) > 0;
        }

        public async Task<bool> Update(TrackerViewModel viewModel)
        {
            return await _unitOfWork.TrackerRepository.Edit((Tracker)viewModel)>0;
        }

        public async Task<TrackerViewModel?> GetTrackerById(int id)
        {
            var tracker = await _unitOfWork.TrackerRepository.GetById(id);
            if (tracker == null) return new TrackerViewModel();
            return (TrackerViewModel)tracker;
        }

        public async Task<bool> DeleteAll(string? userId=null)
        {
            return await _unitOfWork.TrackerRepository.DeleteAll(userId);
        }
    }
}
