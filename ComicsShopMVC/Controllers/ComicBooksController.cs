using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ComicsShopMVC.Data;
using ComicsShopMVC.Models;
using Microsoft.AspNetCore.Authorization;

namespace ComicsShopMVC.Controllers
{
    public class ComicBooksController : Controller
    {
        private readonly AppDbContext _context;

        public ComicBooksController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var comics = _context.ComicBooks.Include(c => c.Publisher).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                string searchLower = searchString.ToLower();

                comics = comics.Where(s => s.Title.ToLower().Contains(searchLower)
                                        || s.Publisher.Name.ToLower().Contains(searchLower));
            }

            return View(await comics.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            PopulatePublishersDropDownList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ComicBook comicBook)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comicBook);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Comic created successfully!";
                return RedirectToAction(nameof(Index));
            }
            PopulatePublishersDropDownList(comicBook.PublisherId);
            return View(comicBook);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var comic = await _context.ComicBooks.FindAsync(id);
            if (comic != null)
            {
                _context.ComicBooks.Remove(comic);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Comic deleted!";
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteAll()
        {
            _context.ComicBooks.RemoveRange(_context.ComicBooks);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "All comics deleted!";
            return RedirectToAction(nameof(Index));
        }

        private void PopulatePublishersDropDownList(object selectedPublisher = null)
        {
            var publishers = _context.Publishers
                .OrderBy(p => p.Name)
                .ToList();

            var publisherItems = publishers.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = $"{p.Name} (ID: {p.Id})",
                Selected = p.Id == (int?)selectedPublisher
            }).ToList();

            ViewBag.PublisherId = publisherItems;
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var comicBook = await _context.ComicBooks.FindAsync(id);
            if (comicBook == null) return NotFound();

            PopulatePublishersDropDownList(comicBook.PublisherId);
            return View(comicBook);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ComicBook comicBook)
        {
            if (id != comicBook.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comicBook);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Comic updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.ComicBooks.AnyAsync(e => e.Id == comicBook.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            PopulatePublishersDropDownList(comicBook.PublisherId);
            return View(comicBook);
        }
    }
}