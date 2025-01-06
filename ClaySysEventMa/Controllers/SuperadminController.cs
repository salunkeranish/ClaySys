using ClaySysEventMa.Data;
using ClaySysEventMa.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

[Authorize(Roles = "Superadmin, Admin")]
public class SuperadminController : Controller
{
    private readonly DataAccess _dataAccess;

    public SuperadminController(IConfiguration configuration)
    {
        // Initialize DataAccess with configuration
        _dataAccess = new DataAccess(configuration);
    }

    // Display the Superadmin dashboard
    public async Task<IActionResult> Index()
    {
        try
        {
            // Retrieve data for the dashboard
            var users = await _dataAccess.GetUsersAsync();
            var events = await _dataAccess.GetEventsAsync();
            var registrations = await _dataAccess.GetRegistrationsAsync();

            var viewModel = new SuperadminViewModel
            {
                Users = users,
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
            return View(new SuperadminViewModel());
        }
    }

    // Grant admin rights to a user
    [Authorize(Roles = "Superadmin")]
    [HttpPost]
    public async Task<IActionResult> GrantAdmin(int userId)
    {
        try
        {
            var user = await _dataAccess.GetUserByIdAsync(userId);
            if (user != null)
            {
                user.Role = "Admin";
                await _dataAccess.UpdateUserAsync(user);
            }
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in GrantAdmin: {ex.Message}");
            // Add a generic error message to ModelState
            ModelState.AddModelError(string.Empty, "An error occurred while granting admin rights.");
            return RedirectToAction(nameof(Index));
        }
    }

    // Display the edit user form
    public async Task<IActionResult> EditUser(int? id)
    {
        try
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _dataAccess.GetUserByIdAsync(id.Value);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in EditUser (GET): {ex.Message}");
            return NotFound();
        }
    }

    // Handle editing a user
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUser(User user)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _dataAccess.UpdateUserAsync(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in EditUser (POST): {ex.Message}");
            if (!(await UserExists(user.Id)))
            {
                return NotFound();
            }
            ModelState.AddModelError(string.Empty, "An error occurred while editing the user.");
            return View(user);
        }
    }

    // Display the delete user confirmation page
    public async Task<IActionResult> DeleteUser(int? id)
    {
        try
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _dataAccess.GetUserByIdAsync(id.Value);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in DeleteUser (GET): {ex.Message}");
            return NotFound();
        }
    }

    // Handle confirming the deletion of a user
    [HttpPost, ActionName("DeleteUser")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUserConfirmed(int id)
    {
        try
        {
            var user = await _dataAccess.GetUserByIdAsync(id);
            if (user != null)
            {
                Console.WriteLine($"Attempting to delete user with role: {user.Role} and ID: {user.Id}");
               
                    await _dataAccess.DeleteUserAsync(id);
                    Console.WriteLine($"Deleted user with ID: {id}");
                
            }
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in DeleteUserConfirmed: {ex.Message}");
            ModelState.AddModelError(string.Empty, "An error occurred while deleting the user.");
            return RedirectToAction(nameof(Index));
        }
    }


    // Display the create event form
    public IActionResult CreateEvent()
    {
        return View();
    }

    // Handle creating a new event
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateEvent(Event @event, IFormFile Image)
    {
        try
        {
            if (Image != null && Image.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await Image.CopyToAsync(memoryStream);
                    byte[] imageBytes = memoryStream.ToArray();
                    @event.ImageBase64 = Convert.ToBase64String(imageBytes);
                }
                Console.WriteLine("Image has been uploaded and converted to Base64.");
            }
            else
            {
                ModelState.AddModelError("ImageBase64", "The Image field is required.");
                Console.WriteLine("No image uploaded.");
            }

            if (ModelState.IsValid)
            {
                await _dataAccess.AddEventAsync(@event);
                Console.WriteLine("Event has been added successfully.");
                return RedirectToAction(nameof(Index));
            }

            // Log validation errors if any
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }

            return View(@event);
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in CreateEvent: {ex.Message}");
            ModelState.AddModelError(string.Empty, "An error occurred while creating the event.");
            return View(@event);
        }
    }

    // Display the edit event form
    public async Task<IActionResult> EditEvent(int id)
    {
        try
        {
            var eventItem = await _dataAccess.GetEventByIdAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }
            return View(eventItem);
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in EditEvent (GET): {ex.Message}");
            return NotFound();
        }
    }

    // Handle editing an event
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditEvent(int id, Event @event, IFormFile Image)
    {
        try
        {
            if (id != @event.Id)
            {
                return BadRequest();
            }

            // Handle Image Upload
            if (Image != null && Image.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await Image.CopyToAsync(memoryStream);
                    byte[] imageBytes = memoryStream.ToArray();
                    @event.ImageBase64 = Convert.ToBase64String(imageBytes);
                }
                Console.WriteLine("Image uploaded and converted to Base64.");
            }

            if (ModelState.IsValid)
            {
                await _dataAccess.UpdateEventAsync(@event);
                Console.WriteLine("Event updated successfully.");
                return RedirectToAction(nameof(Index));
            }

            // Log validation errors if any
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }

            return View(@event);
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in EditEvent (POST): {ex.Message}");
            ModelState.AddModelError(string.Empty, "An error occurred while editing the event.");
            return View(@event);
        }
    }

    // Display the delete event confirmation page
    public async Task<IActionResult> DeleteEvent(int? id)
    {
        try
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _dataAccess.GetEventByIdAsync(id.Value);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in DeleteEvent (GET): {ex.Message}");
            return NotFound();
        }
    }

    // Handle confirming the deletion of an event
    [HttpPost, ActionName("DeleteEvent")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteEventConfirmed(int id)
    {
        try
        {
            await _dataAccess.DeleteEventAsync(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in DeleteEventConfirmed: {ex.Message}");
            ModelState.AddModelError(string.Empty, "An error occurred while deleting the event.");
            return RedirectToAction(nameof(Index));
        }
    }

    // Display the event details
    public async Task<IActionResult> EventDetails(int id)
    {
        try
        {
            var @event = await _dataAccess.GetEventByIdAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in EventDetails: {ex.Message}");
            return NotFound();
        }
    }
    // Check if an event exists
    private async Task<bool> EventExists(int id)
    {
        try
        {
            // Retrieve the event by ID
            var @event = await _dataAccess.GetEventByIdAsync(id);
            // Return true if the event exists, false otherwise
            return @event != null;
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in EventExists: {ex.Message}");
            return false;
        }
    }
    // Display the remove registration confirmation page
    [Authorize(Roles = "Superadmin, Admin")]
    public async Task<IActionResult> RemoveRegistration(int? id)
    {
        try
        {
            if (id == null)
            {
                return NotFound();
            }
            var registration = (await _dataAccess.GetRegistrationsAsync()).FirstOrDefault(r => r.Id == id);
            if (registration == null)
            {
                return NotFound();
            }
            return View(registration);
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in RemoveRegistration (GET): {ex.Message}");
            return NotFound();
        }
    }

    // Handle confirming the removal of a registration
    [Authorize(Roles = "Superadmin, Admin")]
    [HttpPost, ActionName("RemoveRegistration")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveRegistrationConfirmed(int id)
    {
        try
        {
            await _dataAccess.RemoveRegistrationAsync(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in RemoveRegistrationConfirmed: {ex.Message}");
            ModelState.AddModelError(string.Empty, "An error occurred while removing the registration.");
            return RedirectToAction(nameof(Index));
        }
    }
    // Check if a user exists
    private async Task<bool> UserExists(int id)
    {
        try
        {
            // Retrieve the user by ID
            var user = await _dataAccess.GetUserByIdAsync(id);
            // Return true if the user exists, false otherwise
            return user != null;
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in UserExists: {ex.Message}");
            return false;
        }
    }
    // Display the list of registered users
    public async Task<IActionResult> RegisteredUsers()
    {
        try
        {
            // Retrieve the list of registered users
            var registration = await _dataAccess.GetRegistrationsAsync();
            // Return the view with the registered users
            return View(registration);
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in RegisteredUsers: {ex.Message}");
            // Add a generic error message to ModelState
            ModelState.AddModelError(string.Empty, "An error occurred while loading the registered users.");
            // Return an empty view in case of an error
            return View(new List<Registration>());
        }
    }
}
