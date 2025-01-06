using ClaySysEventMa.Data;
using ClaySysEventMa.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

[Authorize(Roles = "User, Admin")]
public class UserController : Controller
{
    private readonly DataAccess _dataAccess;

    public UserController(IConfiguration configuration)
    {
        // Initialize DataAccess with configuration
        _dataAccess = new DataAccess(configuration);
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
                // Redirect to the UserEvent index after successful update
                return RedirectToAction(nameof(Index), "UserEvent");
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

    // Display the change password form
    public IActionResult ChangePassword()
    {
        return View();
    }

    // Handle changing the user's password
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword)
    {
        try
        {
            // Authenticate the user with the old password
            var user = await _dataAccess.GetUserAsync(User.Identity.Name, oldPassword);
            if (user == null)
            {
                // Add a validation error if the old password is incorrect
                ModelState.AddModelError(string.Empty, "Incorrect old password.");
                return View();
            }

            // Update the user's password
            user.Password = newPassword;
            await _dataAccess.UpdateUserAsync(user);
            // Redirect to the UserEvent index after successful password change
            return RedirectToAction(nameof(Index), "UserEvent");
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in ChangePassword (POST): {ex.Message}");
            // Add a generic error message to ModelState
            ModelState.AddModelError(string.Empty, "An error occurred while changing the password.");
            return View();
        }
    }
}
