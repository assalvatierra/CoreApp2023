using eJobv30.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealSys.CoreLib.Interfaces.System;
using RealSys.CoreLib.Models.Erp;
using RealSys.CoreLib.Models.SysDB;
using System.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<eJobContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("eJobContext") ?? throw new InvalidOperationException("Connection string 'eJobContext' not found.")));

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("AuthenticationConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


//RealSys - system dependencies
builder.Services.AddDbContext<SysDBContext>(options =>
    options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")
        ));


//RealSys - erp db
builder.Services.AddDbContext<ErpDbContext>(options =>
    options.UseSqlServer(
            builder.Configuration.GetConnectionString("ErpDbContext")
        ));


builder.Services.AddScoped<ISystemServices, RealSys.Modules.SysLib.SystemServices>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
    endpoints.MapControllerRoute(
      name: "default",
      pattern: "{controller=Home}/{action=Index}/{id?}"
    );
});

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapRazorPages();



app.Run();
