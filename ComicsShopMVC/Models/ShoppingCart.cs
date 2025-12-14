using Microsoft.EntityFrameworkCore;
using ComicsShopMVC.Data;

namespace ComicsShopMVC.Models
{
    public class ShoppingCart
    {
        private readonly AppDbContext _context;

        public string? ShoppingCartId { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; } = default!;

        private ShoppingCart(AppDbContext context)
        {
            _context = context;
        }

        public static ShoppingCart GetCart(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.Session;
            AppDbContext context = services.GetService<AppDbContext>() ?? throw new Exception("Error initializing");

            string cartId = session?.GetString("CartId") ?? Guid.NewGuid().ToString();
            session?.SetString("CartId", cartId);

            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }

        public void AddToCart(ComicBook comic)
        {
            var shoppingCartItem = _context.ShoppingCartItems.SingleOrDefault(
                s => s.ComicBook.Id == comic.Id && s.ShoppingCartId == ShoppingCartId);

            var currentAmountInCart = shoppingCartItem?.Amount ?? 0;

            if (currentAmountInCart + 1 > comic.QuantityInStock)
            {
                return;
            }

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    ComicBook = comic,
                    Amount = 1
                };
                _context.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }
            _context.SaveChanges();
        }

        public int RemoveFromCart(ComicBook comic)
        {
            var shoppingCartItem = _context.ShoppingCartItems.SingleOrDefault(
                s => s.ComicBook.Id == comic.Id && s.ShoppingCartId == ShoppingCartId);

            var localAmount = 0;

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                    localAmount = shoppingCartItem.Amount;
                }
                else
                {
                    _context.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }
            _context.SaveChanges();
            return localAmount;
        }

        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems ??= _context.ShoppingCartItems
                .Where(c => c.ShoppingCartId == ShoppingCartId)
                .Include(s => s.ComicBook)
                .ToList();
        }

        public decimal GetShoppingCartTotal()
        {
            return _context.ShoppingCartItems
                .Where(c => c.ShoppingCartId == ShoppingCartId)
                .Select(c => c.ComicBook.Price * c.Amount)
                .Sum();
        }

        public void ClearCart()
        {
            var cartItems = _context.ShoppingCartItems
                .Where(cart => cart.ShoppingCartId == ShoppingCartId);

            _context.ShoppingCartItems.RemoveRange(cartItems);
            _context.SaveChanges();
        }
    }
}