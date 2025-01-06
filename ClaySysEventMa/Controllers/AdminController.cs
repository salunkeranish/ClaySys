using Microsoft.AspNetCore.Mvc;
using ClaySysEventMa.Models;
using ClaySysEventMa.Data;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly DataAccess _dataAccess;

    public AdminController(IConfiguration configuration)
    {
        // Initialize DataAccess with configuration
        _dataAccess = new DataAccess(configuration);
    }

    // Display the admin dashboard
    public async Task<IActionResult> Index()
    {
        try
        {
            // Retrieve data for the dashboard
            var viewModel = new SuperadminViewModel
            {
                Users = await _dataAccess.GetUsersAsync(),
                Events = await _dataAccess.GetEventsAsync(),
                Registrations = await _dataAccess.GetRegistrationsAsync()
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
    // Display the create event form
    public IActionResult CreateEvent()
    {
        return View();
    }

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

    // Display the edit user form
    public async Task<IActionResult> EditUser(int? id)
    {
        try
        {
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve the user by ID
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
                // Update the user in the database
                await _dataAccess.UpdateUserAsync(user);
                // Redirect to the admin dashboard after successful update
                return RedirectToAction(nameof(Index));
            }
            // Return the view with the current model state to display validation errors
            return View(user);
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in EditUser (POST): {ex.Message}");
            // Add a generic error message to ModelState
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

            // Retrieve the user by ID
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
            // Delete the user from the database
            await _dataAccess.DeleteUserAsync(id);
            // Redirect to the admin dashboard after successful deletion
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in DeleteUserConfirmed: {ex.Message}");
            // Add a generic error message to ModelState
            ModelState.AddModelError(string.Empty, "An error occurred while deleting the user.");
            return RedirectToAction(nameof(Index));
        }
    }

    // Display the remove registration confirmation page
    [Authorize(Roles = "Superadmin, Admin")]
    public async Task<IActionResult> RemoveRegistration(int id)
    {
        try
        {
            // Retrieve the registration by ID
            var registration = await _dataAccess.GetRegistrationByIdAsync(id);
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
            // Remove the registration from the database
            await _dataAccess.RemoveRegistrationAsync(id);
            // Redirect to the admin dashboard after successful removal
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in RemoveRegistrationConfirmed: {ex.Message}");
            // Add a generic error message to ModelState
            ModelState.AddModelError(string.Empty, "An error occurred while removing the registration.");
            return RedirectToAction(nameof(Index));
        }
    }

    // Display the list of registered users
    public async Task<IActionResult> RegisteredUsers()
    {
        try
        {
            // Retrieve the list of registered users
            var registeredUsers = await _dataAccess.GetRegistrationsAsync();
            // Return the view with the registered users
            return View(registeredUsers);
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