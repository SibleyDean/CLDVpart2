using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using eventEasefour.Data;
using eventEasefour.Models;
using System.IO;
using Microsoft.Extensions.Logging;

namespace eventEasefour.Controllers
{
    public class VenuesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogger<VenuesController> _logger;

        public VenuesController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment, ILogger<VenuesController> logger)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        // ✅ GET: Venues (Display All)
        public async Task<IActionResult> Index()
        {
            try
            {
                var venues = await _context.Venues.ToListAsync();
                return View(venues);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading venues.");
                TempData["ErrorMessage"] = "An error occurred while loading venues.";
                return RedirectToAction("Error", "Home");
            }
        }

        // ✅ POST: Venues/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Venue venue, IFormFile imageFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var imageUrl = await SaveImageAsync(imageFile);
                        venue.ImageUrl = imageUrl;
                    }

                    _context.Venues.Add(venue);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Venue created successfully.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating venue.");
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
            }

            return View(venue);
        }

        // ✅ Image Upload Helper
        private async Task<string?> SaveImageAsync(IFormFile imageFile)
        {
            try
            {
                var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                return $"/uploads/{uniqueFileName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving image file.");
                return null;
            }
        }
    }
}
