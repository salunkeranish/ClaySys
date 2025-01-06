using ClaySysEventMa.Data;
using ClaySysEventMa.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

[Authorize(Roles = "User, Admin")]
public class UserEventController : Controller
{
    private readonly DataAccess _dataAccess;

    public UserEventController(IConfiguration configuration)
    {
        // Initialize DataAccess with configuration
        _dataAccess = new DataAccess(configuration);
    }

    // Display the user event dashboard
    public async Task<IActionResult> Index()
    {
        try
        {
            // Retrieve events and user information
            var events = await _dataAccess.GetEventsAsync();
            var user = await _dataAccess.GetUserAsync(User.Identity.Name);
            var userId = user?.Id;
            if (userId == null)
            {
                return NotFound();
            }

            // Retrieve registrations for the user
            var registrations = (await _dataAccess.GetRegistrationsAsync())
                .Where(r => r.UserId == userId)
                .ToList();

            // Create the view model
            var viewModel = new UserDashboardViewModel
            {
                Events = events,
                Registrations = registrations
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in Index: {ex.Message}");
            // Add a generic error message to ModelState
            ModelState.AddModelError(string.Empty, "An error occurred while loading the dashboard.");
            // Return an empty view in case of an error
            return View(new UserDashboardViewModel());
        }
    }

    // Handle registering for an event
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterForEvent(int eventId)
    {
        try
        {
            // Retrieve user information
            var user = await _dataAccess.GetUserAsync(User.Identity.Name);
            var userId = user?.Id;
            if (userId == null)
            {
                return NotFound();
            }

            // Check if the user has already registered for an event
            var existingRegistration = (await _dataAccess.GetRegistrationsAsync())
                .Any(r => r.UserId == userId);

            if (existingRegistration)
            {
                ViewBag.Message = "You can only register for one event.";
                return View("Index", await GetUserDashboardViewModel());
            }

            // Create a new registration
            var registration = new Registration
            {
                UserId = userId.Value,
                EventId = eventId,
                RegistrationDate = DateTime.Now
            };

            // Add the registration to the database
            await _dataAccess.AddRegistrationAsync(registration);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in RegisterForEvent: {ex.Message}");
            // Add a generic error message to ModelState
            ModelState.AddModelError(string.Empty, "An error occurred while registering for the event.");
            return View("Index", await GetUserDashboardViewModel());
        }
    }

    // Retrieve the user dashboard view model
    private async Task<UserDashboardViewModel> GetUserDashboardViewModel()
    {
        try
        {
            // Retrieve events and user information
            var events = await _dataAccess.GetEventsAsync();
            var user = await _dataAccess.GetUserAsync(User.Identity.Name);
            var userId = user?.Id;
            if (userId == null)
            {
                return new UserDashboardViewModel();
            }

            // Retrieve registrations for the user
            var registrations = (await _dataAccess.GetRegistrationsAsync())
                .Where(r => r.UserId == userId)
                .ToList();

            // Create the view model
            var viewModel = new UserDashboardViewModel
            {
                Events = events,
                Registrations = registrations
            };

            return viewModel;
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in GetUserDashboardViewModel: {ex.Message}");
            // Return an empty view model in case of an error
            return new UserDashboardViewModel();
        }
    }
}
