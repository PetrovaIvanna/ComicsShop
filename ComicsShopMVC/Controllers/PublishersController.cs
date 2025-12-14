using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComicsShopMVC.Data;
using ComicsShopMVC.Models;
using Microsoft.AspNetCore.Authorization;

namespace ComicsShopMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PublishersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public PublishersController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(int? searchId)
        {
            if (searchId.HasValue)
            {
                var publisher = await _context.Publishers
                    .FirstOrDefaultAsync(p => p.Id == searchId.Value);

                if (publisher != null)
                {
                    return View(new List<Publisher> { publisher });
                }
                TempData["ErrorMessage"] = $"Publisher with ID {searchId} not found.";
            }
            return View(await _context.Publishers.ToListAsync());
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Publisher publisher)
        {
            var bannedCountries = _configuration.GetSection("BusinessSettings:BannedCountries").Get<string[]>();

            if (bannedCountries != null && publisher.Country != null)
            {
                if (bannedCountries.Any(c => c.Equals(publisher.Country, StringComparison.OrdinalIgnoreCase)))
                {
                    ModelState.AddModelError("Country", $"Publishers from {publisher.Country} are banned due to sanctions.");
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(publisher);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Publisher created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(publisher);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher != null)
            {
                var hasComics = await _context.ComicBooks.AnyAsync(c => c.PublisherId == id);
                if (hasComics)
                {
                    TempData["ErrorMessage"] = "Cannot delete publisher. They have comics linked!";
                    return RedirectToAction(nameof(Index));
                }

                _context.Publishers.Remove(publisher);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Publisher deleted!";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAll()
        {
            _context.ComicBooks.RemoveRange(_context.ComicBooks);
            _context.Publishers.RemoveRange(_context.Publishers);

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "All Data (Publishers & Comics) deleted!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher == null) return NotFound();

            return View(publisher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Publisher publisher)
        {
            if (id != publisher.Id) return NotFound();

            var bannedCountries = _configuration.GetSection("BusinessSettings:BannedCountries").Get<string[]>();
            if (bannedCountries != null && publisher.Country != null)
            {
                if (bannedCountries.Any(c => c.Equals(publisher.Country, StringComparison.OrdinalIgnoreCase)))
                {
                    ModelState.AddModelError("Country", $"Publishers from {publisher.Country} are banned due to sanctions.");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(publisher);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Publisher updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Publishers.AnyAsync(e => e.Id == publisher.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(publisher);
        }
    }
}