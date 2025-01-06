using ClaySysEventMa.Data;
using ClaySysEventMa.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

[Authorize(Roles = "User, Admin")]
public class UserProfileController : Controller
{
    private readonly DataAccess _dataAccess;

    public UserProfileController(IConfiguration configuration)
    {
        // Initialize DataAccess with configuration
        _dataAccess = new DataAccess(configuration);
    }

    // Display the user profile
    public async Task<IActionResult> Index()
    {
        try
        {
            // Retrieve the user by username
            var user = await _dataAccess.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in Index: {ex.Message}");
            return NotFound();
        }
    }

    // Display the edit profile form
    public async Task<IActionResult> EditProfile()
    {
        try
        {
            // Retrieve the user by username
            var user = await _dataAccess.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in EditProfile (GET): {ex.Message}");
            return NotFound();
        }
    }

    // Handle editing the user's profile
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProfile(User user)
    {
        try
        {
            if (ModelState.IsValid)
            {
                // Update the user in the database
                await _dataAccess.UpdateUserAsync(user);
                // Redirect to the UserProfile index after successful update
                return RedirectToAction(nameof(Index));
            }
            // Return the view with the current model state to display validation errors
            return View(user);
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in EditProfile (POST): {ex.Message}");
            // Add a generic error message to ModelState
            ModelState.AddModelError(string.Empty, "An error occurred while editing the profile.");
            return View(user);
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
}
