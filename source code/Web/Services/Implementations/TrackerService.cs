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

        public async Task<IEnumerable<TrackerViewModel>> GetAllTrackers()
        {
            var result = await _unitOfWork.TrackerRepository.GetAll();
            return result.Select(tracker => (TrackerViewModel)tracker);
        }

        public async Task<bool> Delete(int Id)
        {
            await _unitOfWork.TrackerRepository.Delete(Id);
            return await _unitOfWork.SaveAsync();
        }
    }
}
