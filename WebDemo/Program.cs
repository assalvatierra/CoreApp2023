using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using CoreLib.Models;
using CoreLib.Interfaces;
using CoreLib;
using WebDemo.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<WebDemoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'WebDemoContext' not found.")));

builder.Services.AddDbContext<CoreDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'SampleWebContext' not found.")));

//Dependencies
builder.Services.AddScoped<ISupplierService, SupplierLib.SupplierServices>();
//builder.Services.AddScoped<ISupplierService, NotImplementedModules.SupplierServices>();

builder.Services.AddScoped<IMainService, MainServices>();

//services.AddScoped<PageConfigShared.Interfaces.IPageConfigServices, PageConfigService.PageConfigServices>(x =>
//    new PageConfigService.PageConfigServices(tenantcode, targetVersion)
//);


// Add services to the container.
builder.Services.AddControllersWithViews();

//Added this code to avoid JsonException: A possible object cycle was detected.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
