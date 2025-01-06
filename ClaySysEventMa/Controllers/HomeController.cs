using System.Diagnostics;
using ClaySysEventMa.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClaySysEventM.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            // Initialize the logger
            _logger = logger;
        }

        // Display the home page
        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError($"Error in Index: {ex.Message}");
                // Redirect to the error page
                return RedirectToAction(nameof(Error));
            }
        }

        // Display the privacy policy page
        public IActionResult Privacy()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError($"Error in Privacy: {ex.Message}");
                // Redirect to the error page
                return RedirectToAction(nameof(Error));
            }
        }

        // Display the error page with request ID
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            try
            {
                var errorViewModel = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
                return View(errorViewModel);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError($"Error in Error: {ex.Message}");
                return View(new ErrorViewModel());
            }
        }

        // Display the About Us page
        public IActionResult AboutUs()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError($"Error in AboutUs: {ex.Message}");
                // Redirect to the error page
                return RedirectToAction(nameof(Error));
            }
        }

        // Display the Contact Us page
        public IActionResult ContactUs()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError($"Error in ContactUs (GET): {ex.Message}");
                // Redirect to the error page
                return RedirectToAction(nameof(Error));
            }
        }

        // Handle Contact Us form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ContactUs(ContactFormModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Display a success message
                    ViewData["Message"] = "Thank you for contacting us. We will get back to you soon.";
                    return View();
                }
                // Return the view with the current model state to display validation errors
                return View(model);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError($"Error in ContactUs (POST): {ex.Message}");
                // Add a generic error message to ModelState
                ModelState.AddModelError(string.Empty, "An error occurred while submitting your contact request.");
                return View(model);
            }
        }
    }
}
