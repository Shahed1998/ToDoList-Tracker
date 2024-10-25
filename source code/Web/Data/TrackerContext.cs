using Microsoft.EntityFrameworkCore;
using Web.Models.General_Entities;

namespace Web.Data
{
    public class TrackerContext : DbContext
    {
        public TrackerContext(DbContextOptions<TrackerContext> options) : base(options) { }

        #region DbSet
        public DbSet<Tracker> Trackers { get; set; }
        public DbSet<AchievementResult> AchievementResults { get; set; }
        #endregion
    }
}
