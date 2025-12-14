using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ComicsShopMVC.Models;
using ComicsShopMVC.Data;

namespace ComicsShopMVC.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ShoppingCart _shoppingCart;

        public OrderController(AppDbContext context, ShoppingCart shoppingCart)
        {
            _context = context;
            _shoppingCart = shoppingCart;
        }

        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            if (_shoppingCart.ShoppingCartItems.Count == 0)
            {
                ModelState.AddModelError("", "Your cart is empty, add some comics first");
            }

            if (ModelState.IsValid)
            {
                order.OrderPlaced = DateTime.Now;
                order.OrderTotal = _shoppingCart.GetShoppingCartTotal();

                _context.Orders.Add(order);
                _context.SaveChanges();

                foreach (var item in items)
                {
                    var orderDetail = new OrderDetail
                    {
                        Amount = item.Amount,
                        Price = item.ComicBook.Price,
                        ComicBookId = item.ComicBook.Id,
                        OrderId = order.OrderId
                    };
                    _context.OrderDetails.Add(orderDetail);
                }

                foreach (var item in items)
                {
                    var comic = _context.ComicBooks.Find(item.ComicBook.Id);
                    if (comic != null)
                    {
                        comic.QuantityInStock -= item.Amount;
                    }
                }
                _context.SaveChanges();

                _shoppingCart.ClearCart();

                return RedirectToAction("CheckoutComplete");
            }

            return View(order);
        }

        public IActionResult CheckoutComplete()
        {
            ViewBag.CheckoutCompleteMessage = "Thanks for your order. You'll soon get your comics!";
            return View();
        }
    }
}