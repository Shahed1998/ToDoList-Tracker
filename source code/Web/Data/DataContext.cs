using Microsoft.EntityFrameworkCore;
using Web.Models.General_Entities;

namespace web.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}

        public DbSet<Tracker> Trackers { get; set; }
    }
}
