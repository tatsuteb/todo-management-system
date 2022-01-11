using Infrastructure.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebClient;
using WebClient.Data;
using WebClient.Models.Shared.Exceptions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppIdentityDbContext>(options =>
    options.UseInMemoryDatabase("todo_management_system_identity_db"));
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("todo_management_system_db"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    {
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase= false;
            options.Password.RequiredLength = 1;
        }

        options.SignIn.RequireConfirmedAccount = true;
    })
    .AddEntityFrameworkStores<AppIdentityDbContext>();
builder.Services.AddControllersWithViews(options =>
    options.Filters.Add(typeof(WebClientExceptionFilter)));

SetUpDependencyInjections.SetUp(builder);

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
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Todo}/{action=Index}");
app.MapRazorPages();

app.Run();
