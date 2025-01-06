using Microsoft.AspNetCore.Mvc;
using ClaySysEventMa.Data;
using ClaySysEventMa.Models;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

[Authorize(Roles = "Admin")]
public class AdminEventController : Controller
{
    private readonly DataAccess _dataAccess;

    public AdminEventController(IConfiguration configuration)
    {
        // Initialize DataAccess with configuration
        _dataAccess = new DataAccess(configuration);
    }

    // GET: AdminEvent/Index
    public async Task<IActionResult> Index()
    {
        try
        {
            // Retrieve the list of events
            var events = await _dataAccess.GetEventsAsync();
            // Return the view with the events
            return View(events);
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in Index: {ex.Message}");
            // Add a generic error message to ModelState
            ModelState.AddModelError(string.Empty, "An error occurred while loading events.");
            // Return an empty view in case of an error
            return View(new List<Event>());
        }
    }

    // GET: AdminEvent/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: AdminEvent/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Event @event)
    {
        try
        {
            if (ModelState.IsValid)
            {
                // Add the event to the database
                await _dataAccess.AddEventAsync(@event);
                // Redirect to the event list after successful creation
                return RedirectToAction(nameof(Index));
            }
            // Return the view with the current model state to display validation errors
            return View(@event);
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in Create: {ex.Message}");
            // Add a generic error message to ModelState
            ModelState.AddModelError(string.Empty, "An error occurred while creating the event.");
            return View(@event);
        }
    }

    // GET: AdminEvent/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        try
        {
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve the event by ID
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
            Console.WriteLine($"Error in Edit (GET): {ex.Message}");
            return NotFound();
        }
    }

    // POST: AdminEvent/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Event @event)
    {
        try
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Update the event in the database
                await _dataAccess.UpdateEventAsync(@event);
                // Redirect to the event list after successful update
                return RedirectToAction(nameof(Index));
            }
            // Return the view with the current model state to display validation errors
            return View(@event);
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in Edit (POST): {ex.Message}");
            // Add a generic error message to ModelState
            ModelState.AddModelError(string.Empty, "An error occurred while editing the event.");
            return View(@event);
        }
    }

    // GET: AdminEvent/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        try
        {
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve the event by ID
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
            Console.WriteLine($"Error in Delete (GET): {ex.Message}");
            return NotFound();
        }
    }

    // POST: AdminEvent/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            // Delete the event from the database
            await _dataAccess.DeleteEventAsync(id);
            // Redirect to the event list after successful deletion
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in DeleteConfirmed: {ex.Message}");
            // Add a generic error message to ModelState
            ModelState.AddModelError(string.Empty, "An error occurred while deleting the event.");
            return RedirectToAction(nameof(Index));
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
}
