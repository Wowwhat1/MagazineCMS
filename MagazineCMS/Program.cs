using MagazineCMS.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MagazineCMS.Models;
using MagazineCMS.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using MagazineCMS.DataAccess.DBInitializer;
using MagazineCMS.DataAccess.Repository.IRepository;
using MagazineCMS.DataAccess.Repository;
using System.Text.Json.Serialization;
using MagazineCMS.Services;
using MagazineCMS.Services.BackgroundService;
using Hangfire;
using System.Configuration;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, MagazineCMS.Services.EmailSender>();
builder.Services.AddTransient<MagazineCMS.Services.IEmailSender, MagazineCMS.Services.EmailSender>();
builder.Services.AddControllersWithViews();
builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false).
    AddEntityFrameworkStores<ApplicationDbContext>().
    AddDefaultTokenProviders();
builder.Services.AddScoped<IRepository<Magazine>, MagazineRepository>();
builder.Services.AddRazorPages();
builder.Services.AddScoped<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, MagazineCMS.Services.EmailSender>();
// Add scoped
builder.Services.AddScoped<IDBInitializer, DBInitializer>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<INotificationSender, NotificationSender>();

// Add Hangfire services
builder.Services.AddHangfire(config => config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Hangfire server
builder.Services.AddHangfireServer();

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
app.MapRazorPages();

SeedDatabase();

app.MapControllerRoute(
    name: "areaRoute",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDBInitializer>();
        dbInitializer.Initialize();
    }
}

void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
{
    // Initialize Hangfire
    app.UseHangfireDashboard();
    app.UseHangfireServer();

    // Schedule the SendContributionReminders method to run daily
    RecurringJob.AddOrUpdate<NotificationSender>("ContributionReminderJob", x => x.SendContributionReminders(), Cron.Daily);
}