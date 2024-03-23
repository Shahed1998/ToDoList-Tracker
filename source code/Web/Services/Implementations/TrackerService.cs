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

        public async Task<(Pager<TrackerViewModel>, decimal?)> GetAllTrackers(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _unitOfWork.TrackerRepository.GetAll(pageNumber, pageSize);

            var lst = result.Item1.Select(tracker => (TrackerViewModel)tracker).ToList();

            var test = new Pager<TrackerViewModel>(lst, result.Item1.TotalCount, pageNumber, pageSize);

            return (test, result.Item2);
        }

        public async Task<bool> Delete(int Id)
        {
            await _unitOfWork.TrackerRepository.Delete(Id);
            return await _unitOfWork.SaveAsync();
        }
    }
}
