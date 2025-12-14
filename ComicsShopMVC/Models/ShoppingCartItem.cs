namespace ComicsShopMVC.Models
{
    public class ShoppingCartItem
    {
        public int ShoppingCartItemId { get; set; }
        public ComicBook ComicBook { get; set; } = default!;
        public int Amount { get; set; }
        public string? ShoppingCartId { get; set; }
    }
}