using Microsoft.AspNetCore.Mvc;
using ComicsShopMVC.Models;
using ComicsShopMVC.Data;

namespace ComicsShopMVC.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ShoppingCart _shoppingCart;

        public ShoppingCartController(AppDbContext context, ShoppingCart shoppingCart)
        {
            _context = context;
            _shoppingCart = shoppingCart;
        }

        public IActionResult Index()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;
            return View(_shoppingCart);
        }

        public RedirectToActionResult AddToShoppingCart(int comicId)
        {
            var selectedComic = _context.ComicBooks.FirstOrDefault(p => p.Id == comicId);
            if (selectedComic != null)
            {
                _shoppingCart.AddToCart(selectedComic);
            }
            return RedirectToAction("Index");
        }

        public RedirectToActionResult RemoveFromShoppingCart(int comicId)
        {
            var selectedComic = _context.ComicBooks.FirstOrDefault(p => p.Id == comicId);
            if (selectedComic != null)
            {
                _shoppingCart.RemoveFromCart(selectedComic);
            }
            return RedirectToAction("Index");
        }
    }
}