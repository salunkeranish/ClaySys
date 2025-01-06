using Microsoft.AspNetCore.Mvc;
using ClaySysEventMa.Models;
using ClaySysEventMa.Data;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

public class AccountController : Controller
{
    private readonly DataAccess _dataAccess;

    public AccountController(IConfiguration configuration)
    {
        // Initialize DataAccess with configuration
        _dataAccess = new DataAccess(configuration);
    }

    // Display the registration form
    public IActionResult Register()
    {
        return View();
    }

    // Handle user registration
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(User user)
    {
        try
        {
            if (ModelState.IsValid)
            {
                user.Role = "User";
                await _dataAccess.AddUserAsync(user);
                // Redirect to the login page 
                return RedirectToAction(nameof(Login));
            }
            // Return the view with the current model state to display validation errors
            return View(user);
        }
        catch (Exception ex)
        { 
            Console.WriteLine($"Error in Register: {ex.Message}");
            ModelState.AddModelError(string.Empty, "An error occurred while registering the user.");
            return View(user);
        }
    }

    // Display the grant admin form (accessible only to Superadmin)
    [Authorize(Roles = "Superadmin")]
    public IActionResult GrantAdmin()
    {
        return View();
    }

    // Handle granting admin rights (accessible only to Superadmin)
    [HttpPost]
    [Authorize(Roles = "Superadmin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GrantAdmin(int userId)
    {
        try
        {
            // Update the user role to Admin
            await _dataAccess.UpdateUserRoleAsync(userId, "Admin");
            return RedirectToAction(nameof(Index), "Superadmin");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GrantAdmin: {ex.Message}");
            ModelState.AddModelError(string.Empty, "An error occurred while granting admin rights.");
            return View();
        }
    }

    // Display the login form
    public IActionResult Login()
    {
        return View();
    }

    // Handle user login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string username, string password)
    {
        try
        {
            // Authenticate the user
            var user = await _dataAccess.GetUserAsync(username, password);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View();
            }

            // Create the user claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            // Create the claims identity
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Sign in the user with the created claims
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            // Redirect based on the user role
            switch (user.Role)
            {
                case "Admin":
                    return RedirectToAction("Index", "Admin");
                case "Superadmin":
                    return RedirectToAction("Index", "Superadmin");
                case "User":
                    return RedirectToAction("Index", "UserEvent");
                default:
                    return RedirectToAction(nameof(Index), "Home");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in Login: {ex.Message}");
            ModelState.AddModelError(string.Empty, "An error occurred while logging in.");
            // Return the view 
            return View();
        }
    }

    // Handle user logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        try
        {
            // Sign out the user
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // Redirect to the login page after logout
            return RedirectToAction(nameof(Login));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in Logout: {ex.Message}");
            ModelState.AddModelError(string.Empty, "An error occurred while logging out.");
            return RedirectToAction(nameof(Login));
        }
    }
}
