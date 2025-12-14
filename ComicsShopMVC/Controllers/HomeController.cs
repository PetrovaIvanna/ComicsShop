using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ComicsShopMVC.Models;
using ComicsShopMVC.Data;

namespace ComicsShopMVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;

    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        bool isAdmin = User.IsInRole("Admin");

        ViewBag.IsAdmin = isAdmin;

        if (isAdmin)
        {
            _logger.LogInformation("Admin detected. Loading Dashboard Statistics...");

            ViewBag.TotalComics = await _context.ComicBooks.CountAsync();
            ViewBag.TotalPublishers = await _context.Publishers.CountAsync();

            ViewBag.TotalValue = await _context.ComicBooks.AnyAsync()
                ? await _context.ComicBooks.SumAsync(c => c.Price)
                : 0;

            var recentComics = await _context.ComicBooks
                .Include(c => c.Publisher)
                .OrderByDescending(c => c.Id)
                .Take(5)
                .ToListAsync();

            return View(recentComics);
        }
        else
        {
            return View(new List<ComicBook>());
        }
    }

    public IActionResult GradingGuide()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}