using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using eventEasefour.Data;
using eventEasefour.Models;

namespace eventEasefour.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EventsController> _logger;

        public EventsController(ApplicationDbContext context, ILogger<EventsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ✅ GET: Events (Display All)
        public async Task<IActionResult> Index()
        {
            try
            {
                var events = await _context.Events
                    .Include(e => e.Venue)
                    .OrderByDescending(e => e.EventDate)
                    .ToListAsync();
                return View(events);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading events.");
                TempData["ErrorMessage"] = "An error occurred while loading events.";
                return RedirectToAction("Error", "Home");
            }
        }

        // ✅ GET: Events/Create
        public IActionResult Create()
        {
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName");
            return View();
        }

        // ✅ POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event @event)
        {
            try
            {
                _logger.LogInformation("Create method hit."); // ✅ Debugging log

                // ✅ Ensure Venue Exists
                if (!_context.Venues.Any(v => v.VenueId == @event.VenueId))
                {
                    ModelState.AddModelError("VenueId", "Selected venue does not exist.");
                }

                if (ModelState.IsValid)
                {
                    _context.Events.Add(@event);
                    var saveResult = await _context.SaveChangesAsync();

                    if (saveResult > 0)
                    {
                        _logger.LogInformation("Event created successfully.");
                        TempData["SuccessMessage"] = "Event created successfully.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _logger.LogError("Event was not saved to the database. SaveChangesAsync() returned 0.");
                        TempData["ErrorMessage"] = "Failed to save the event. Please try again.";
                    }
                }
                else
                {
                    // ✅ Log Validation Errors
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        _logger.LogError($"Validation Error: {error.ErrorMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating event.");
                TempData["ErrorMessage"] = "An unexpected error occurred. Please try again.";
            }

            // ✅ Reload Venue List in case of error
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", @event.VenueId);
            return View(@event);
        }
    }
}
