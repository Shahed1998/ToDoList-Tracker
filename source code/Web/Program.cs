using web.Data;
using Microsoft.EntityFrameworkCore;
using Web.Repositories.Implementations;
using Web.Repositories.Interfaces;
using Web.Services.Implementations;
using Web.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using Web.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

#region Database context register
var connectionStrings = builder.Configuration.GetConnectionString("DefaultConnection");
// Identity context
builder.Services.AddDbContext<DataContext>(opt => opt.UseSqlServer(connectionStrings,
    sqlServerOptionsAction: sqlAction =>
    {
        sqlAction.EnableRetryOnFailure(
            maxRetryCount: 10,
            maxRetryDelay: TimeSpan.FromSeconds(5),
            errorNumbersToAdd: null);
    }));
// Tracker context
builder.Services.AddDbContext<TrackerContext>(opt => opt.UseSqlServer(connectionStrings,
    sqlServerOptionsAction: sqlAction =>
    {
        sqlAction.EnableRetryOnFailure(
            maxRetryCount: 10,
            maxRetryDelay: TimeSpan.FromSeconds(5),
            errorNumbersToAdd: null);
    }));
#endregion

#region Identity register
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<DataContext>();
#endregion

#region Unit of work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
#endregion

#region Services
builder.Services.AddScoped<ITrackerService, TrackerService>();
#endregion



var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}");

app.Run();
