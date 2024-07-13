using Microsoft.EntityFrameworkCore;
using web.Data;
using Web.Models.Business_Entities;
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

        public async Task<(Pager<Tracker>, decimal?)> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            string? sql = @"
                                SELECT ISNULL(
                                    NULLIF(
                                        (SELECT 
                                            CASE 
                                                WHEN SUM(t.Planned) = 0 THEN 0 
                                                ELSE CAST(SUM(t.Completed) / SUM(t.Planned) * 100 AS DECIMAL(6,2)) 
                                            END 
                                        FROM Tracker t), 0), 0) as Result";

            decimal? achievement = _dataContext.AchievementResults.FromSqlRaw(sql).AsNoTracking().AsEnumerable().FirstOrDefault()?.Result;

            var pagedList = await Pager<Tracker>.CreateAsync(_dataContext.Trackers.OrderByDescending(x => x.Date), pageNumber, pageSize);

            return (pagedList, achievement);

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
