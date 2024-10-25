using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Xml;
using Web.Models.General_Entities;

namespace web.Data
{
    public class DataContext : IdentityDbContext
    {
        private readonly IConfiguration _configuration;

        public DataContext(DbContextOptions<DataContext> options, IConfiguration configuration) : base(options) 
        {
            _configuration = configuration;
        }


    }
}
