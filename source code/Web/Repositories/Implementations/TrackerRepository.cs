using Microsoft.EntityFrameworkCore;
using web.Data;
using Web.Models.General_Entities;
using Web.Repositories.Interfaces;

namespace Web.Repositories.Implementations
{
    public class TrackerRepository : ITrackerRepository
    {
        public readonly DataContext _dataContext;

        public TrackerRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> Add(Tracker model)
        {
            return await _dataContext.Database.ExecuteSqlAsync($"usp_InsertIntoTrackerTable @completed = {model.Completed}, @planned = {model.Planned}") > 0;
        }

        public async Task<IEnumerable<Tracker>> GetAll()
        {
            return await _dataContext.Trackers.OrderByDescending(x => x.Id).AsNoTracking().ToListAsync();
        }

        public async Task Delete(int Id)
        {
            var entity = await _dataContext.Trackers.FindAsync(Id);
            if (entity != null)
            {
                _dataContext.Trackers.Remove(entity);
            }
        }
    }
}
