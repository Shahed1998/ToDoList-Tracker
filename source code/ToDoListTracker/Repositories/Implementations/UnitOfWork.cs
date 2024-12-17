using web.Data;
using Web.Data;
using Web.Repositories.Interfaces;

namespace Web.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly TrackerContext _trackerContext;
        public UnitOfWork(TrackerContext trackerContext)
        {
            _trackerContext = trackerContext;
        }

        public ITrackerRepository TrackerRepository => new TrackerRepository(_trackerContext);

        public async Task<bool> SaveAsync()
        {
            return await _trackerContext.SaveChangesAsync() > 0;
        }
    }
}
