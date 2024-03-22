using web.Data;
using Web.Repositories.Interfaces;

namespace Web.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly DataContext _dataContext;
        public UnitOfWork(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public ITrackerRepository TrackerRepository => new TrackerRepository(_dataContext);

        public async Task<bool> SaveAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}
