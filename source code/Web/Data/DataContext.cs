using Microsoft.EntityFrameworkCore;
using System.Xml;
using Web.Models.General_Entities;

namespace web.Data
{
    public class DataContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public DataContext(DbContextOptions<DataContext> options, IConfiguration configuration) : base(options) 
        {
            _configuration = configuration;
        }

        public DbSet<Tracker> Trackers { get; set; }
        public DbSet<AchievementResult> AchievementResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AchievementResult>().HasNoKey();
        }
    }
}
