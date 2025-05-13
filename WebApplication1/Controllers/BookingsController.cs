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
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BookingsController> _logger;

        // ✅ Constructor (Dependency Injection)
        public BookingsController(ApplicationDbContext context, ILogger<BookingsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ✅ GET: Bookings (Manage Bookings - Display All)
        public async Task<IActionResult> Index()
        {
            try
            {
                var bookings = await _context.Bookings
                    .Include(b => b.Event)
                    .Include(b => b.Venue)
                    .OrderByDescending(b => b.BookingDate)
                    .ToListAsync();

                return View(bookings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading bookings.");
                return RedirectToAction("Error", "Home");
            }
        }

        // ✅ GET: Bookings/Create
        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventName");
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName");
            return View();
        }

        // ✅ POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Bookings booking)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // ✅ Prevent Booking with Past Date
                    if (booking.BookingDate < DateTime.UtcNow)
                    {
                        ModelState.AddModelError("BookingDate", "Booking date cannot be in the past.");
                        return View(booking);
                    }

                    _context.Add(booking);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating booking.");
                return RedirectToAction("Error", "Home");
            }

            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventName", booking.EventId);
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", booking.VenueId);
            return View(booking);
        }
    }
}
