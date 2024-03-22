using Microsoft.EntityFrameworkCore;
using System.Xml;
using Web.Models.General_Entities;

namespace web.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}

        public DbSet<Tracker> Trackers { get; set; }
        public DbSet<AchievementResult> AchievementResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AchievementResult>().HasNoKey();
        }
    }
}
