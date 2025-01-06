using ClaySysEventMa.Data;
using ClaySysEventMa.Models;
using ClaySysEventMa.Data;
using ClaySysEventMa.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ClaySysEventM
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Configure DataAccess as a singleton.
            builder.Services.AddSingleton<DataAccess>();

            // Add authentication services.
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Enable authentication and authorization.
            app.UseAuthentication();
            app.UseAuthorization();

            // Seed the superadmin during application startup.
            await SeedSuperadminAsync(app.Services, app.Logger);

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }


        /// <param name="serviceProvider">Service provider to resolve dependencies.</param>
        /// <param name="logger">Logger to log information and errors.</param>
        private static async Task SeedSuperadminAsync(IServiceProvider serviceProvider, ILogger logger)
        {
            using var scope = serviceProvider.CreateScope();
            var dataAccess = scope.ServiceProvider.GetRequiredService<DataAccess>();

            try
            {
                var users = await dataAccess.GetUsersAsync();
                if (!users.Any(u => u.Role == "Superadmin"))
                {
                    var superadmin = new User
                    {
                        FirstName = "Super",
                        LastName = "Admin",
                        Username = "superadmin",
                        Password = "SuperadminPassword",
                        EmailAddress = "superadmin@claysys.com",
                        Role = "Superadmin",
                        PhoneNumber = "1234567890",
                        Address = "Head Office",
                        State = "Default State",
                        City = "Default City",
                        Gender = "Other",
                        DateOfBirth = DateTime.Now.AddYears(-30)
                    };

                    await dataAccess.AddUserAsync(superadmin);
                    logger.LogInformation("Superadmin user successfully created.");
                }
                else
                {
                    logger.LogInformation("Superadmin user already exists.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error seeding the Superadmin user.");
                throw;
            }
        }
    }
}
