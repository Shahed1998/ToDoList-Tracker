using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using web.Data;
using Web.Data;
using Web.Helpers;
using Web.Models.Business_Entities;
using Web.Models.General_Entities;
using Web.Repositories.Interfaces;

namespace Web.Repositories.Implementations
{
    public class TrackerRepository : ITrackerRepository
    {
        public readonly TrackerContext _trackerContext;

        public TrackerRepository(TrackerContext trackerContext)
        {
            _trackerContext = trackerContext;
        }

        public async Task<bool> Add(Tracker model)
        {
            return await _trackerContext.Database.ExecuteSqlAsync($"usp_InsertIntoTrackerTable @completed = {model.Completed}, @planned = {model.Planned}, @userId = {model.UserId}") > 0;
        }

        public async Task<(Pager<Tracker>, decimal?)> GetAll(int pageNumber = 1, int pageSize = 10, string? userId = null)
        {
            AchievementResult? achievement;

            if (userId != null)
            {
                achievement = await _trackerContext.AchievementResults
                    .Where(x => x.UserId == userId).FirstOrDefaultAsync();
            }
            else
            {
                achievement = await _trackerContext.AchievementResults.FirstOrDefaultAsync();
            }

            var pagedList = await Pager<Tracker>.CreateAsync(_trackerContext.Trackers
                .Where(x => x.UserId == userId).OrderByDescending(x => x.Date), pageNumber, pageSize);

            return (pagedList, (achievement == null ? Decimal.Zero : achievement.Result));

        }

        public async Task<int> Delete(int Id)
        {
            int rowsUpdated = 0;
            try
            {
                Tracker? tracker = await _trackerContext.Trackers.FindAsync(Id);

                if(tracker != null)
                {
                    var sql = $"DELETE FROM Tracker WHERE Id=@Id";
                    var parameter = new SqlParameter("@Id", Id);
                    rowsUpdated = await _trackerContext.Database.ExecuteSqlRawAsync(sql, parameter);

                    var trackerInfo = "Successfully Deleted Tracker {"+
                        $"Id: {tracker.Id}, Completed: {tracker.Completed}, Planned: {tracker.Planned}," +
                        $" Date: {(tracker.Date != null? tracker.Date.Value.ToString("MMM dd, yyyy"): String.Empty)}" + "}";

                    if(rowsUpdated > 0) HelperSerilog.LogInformation(trackerInfo);
                }
            }
            catch(Exception ex)
            {
                HelperSerilog.LogError(ex.Message, ex);    
            }
            finally
            {
                HelperSerilog.CloseAndFlushLogger();
            }
            return rowsUpdated;
        }

        public async Task<int> Edit(Tracker model)
        {
            var rowsUpdate = 0;
            try
            {
                var tracker = await _trackerContext.Trackers.FindAsync(model.Id);

                if (tracker != null) 
                {
                    model.Percentage = (model.Completed / model.Planned) * 100;

                    var sql = "UPDATE Tracker SET Completed=@Completed, Planned=@Planned, Percentage=@Percentage, Date=@Date WHERE Id=@Id";

                    var parameters = new List<SqlParameter>();

                    parameters.Add(new SqlParameter("@Completed", model.Completed));
                    parameters.Add(new SqlParameter("@Planned", model.Planned));
                    parameters.Add(new SqlParameter("@Percentage", (model.Completed / model.Planned) * 100));
                    parameters.Add(new SqlParameter("@Date", model.Date));
                    parameters.Add(new SqlParameter("@Id", model.Id));

                    rowsUpdate = await _trackerContext.Database.ExecuteSqlRawAsync(sql, parameters);

                    var trackerInfo = "Successfully Updated Tracker from {" +
                        $"Id: {tracker.Id}, Completed: {tracker.Completed}, Planned: {tracker.Planned}," +
                        $" Date: {(tracker.Date != null ? tracker.Date.Value.ToString("MMM dd, yyyy") : String.Empty)}" + "}"+
                        " To {" +
                        $"Id: {model.Id}, Completed: {model.Completed}, Planned: {model.Planned}," +
                        $" Date: {(model.Date != null ? model.Date.Value.ToString("MMM dd, yyyy") : String.Empty)}" + "}";

                    HelperSerilog.LogInformation(trackerInfo);
                }
            }
            catch (Exception ex)
            {
                HelperSerilog.LogError(ex.Message, ex);
            }
            finally
            {
                HelperSerilog.CloseAndFlushLogger();
            }
            return rowsUpdate;
        }

        public async Task<Tracker?> GetById(int id)
        {
            return await _trackerContext.Trackers.FindAsync(id);
        }

        public async Task<bool> DeleteAll(string? userId = null)
        {
            bool success = false;
            try
            {
                var sql = $"EXEC usp_ClearAllTrackers @userId='{userId}'";
                int rowsUpdated = await _trackerContext.Database.ExecuteSqlRawAsync(sql);
                if (rowsUpdated > 0)
                {
                    success = true;

                    var sql2 = $"SELECT UserName FROM AspNetUsers WHERE Id='{userId}'";
                    var username = await _trackerContext.Database.ExecuteSqlRawAsync(sql2);

                    HelperSerilog.LogInformation("Deleted all data on date: " + DateTime.Now.ToString("MMM dd, yyyy hh:mm tt") + " of Username: "+ username);
                }
            }
            catch (Exception ex)
            {
                HelperSerilog.LogError($"{ex.Message}", ex);
            }
            finally { HelperSerilog.CloseAndFlushLogger(); }
            return success;
        }
    }
}
