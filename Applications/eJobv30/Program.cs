using eJobv30.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealSys.CoreLib.Interfaces.System;
using RealSys.CoreLib.Models.Erp;
using RealSys.CoreLib.Models.SysDB;
using System.Configuration;
using Microsoft.Extensions.DependencyInjection;

using DevExpress.AspNetCore;
using DevExpress.AspNetCore.Reporting;
using DevExpress.XtraReports.Web.Extensions;
using eJobv30.Reporting.Services;
using DevExpress.XtraCharts;
using Reporting.Data;
using RealSys.Modules.SysLib;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

//DefaultConnection with secrets pass
var defaultConnection = new SqlConnectionStringBuilder(
        builder.Configuration.GetConnectionString("DefaultConnection"));
defaultConnection.Password = builder.Configuration["DefaultConnectionPassword"];

//eJobContext with secrets pass
var eJobContextString = new SqlConnectionStringBuilder(
        builder.Configuration.GetConnectionString("eJobContext"));
eJobContextString.Password = builder.Configuration["DefaultConnectionPassword"];

//eJobContext with secrets pass
var ErpDbContextString = new SqlConnectionStringBuilder(
        builder.Configuration.GetConnectionString("ErpDbContext"));
ErpDbContextString.Password = builder.Configuration["DefaultConnectionPassword"];

//eJobContext with secrets pass
var EErpContactsContextString = new SqlConnectionStringBuilder(
        builder.Configuration.GetConnectionString("ErpContactsContext"));
EErpContactsContextString.Password = builder.Configuration["DefaultConnectionPassword"];

//AuthenticationConnection
var AuthenticationConnectionString = new SqlConnectionStringBuilder(
        builder.Configuration.GetConnectionString("AuthenticationConnection"));
AuthenticationConnectionString.Password = builder.Configuration["DefaultConnectionPassword"];

//Start db connections

builder.Services.AddDbContext<ErpContactsContext>(options =>
    options.UseSqlServer(EErpContactsContextString.ConnectionString ?? 
    throw new InvalidOperationException("Connection string 'ErpContactsContext' not found.")));

//Devexpress 
builder.Services.AddDevExpressControls();
builder.Services.AddScoped<ReportStorageWebExtension, CustomReportStorageWebExtension>();
builder.Services.AddDbContext<ReportDbContext>(options =>
    options.UseSqlServer(defaultConnection.ConnectionString));


builder.Services.AddDbContext<eJobContext>(options =>
    options.UseSqlServer(eJobContextString.ConnectionString ?? 
    throw new InvalidOperationException("Connection string 'eJobContext' not found.")));

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(AuthenticationConnectionString.ConnectionString));


//RealSys - system dependencies
builder.Services.AddDbContext<SysDBContext>(options =>
    options.UseSqlServer(
           defaultConnection.ConnectionString
        ));


//RealSys - erp db
builder.Services.AddDbContext<ErpDbContext>(options =>
    options.UseSqlServer(
            defaultConnection.ConnectionString
        ));

builder.Services.AddDbContext<ErpContactsContext>(options =>
    options.UseSqlServer(
            defaultConnection.ConnectionString
        ));


builder.Services.AddScoped<RealSys.CoreLib.Interfaces.IReportRepo,
    RealSys.Modules.Reports.ReportRepo>();
builder.Services.AddScoped<RealSys.CoreLib.Services.ReportServices,
    RealSys.CoreLib.Services.ReportServices>();

builder.Services.AddScoped<RealSys.CoreLib.Interfaces.System.ISystemServices,
    RealSys.Modules.SysLib.SystemServices>();




builder.Services.AddScoped<ISystemServices, RealSys.Modules.SysLib.SystemServices>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(10);//You can set Time   
});

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

app.UseSession();

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
