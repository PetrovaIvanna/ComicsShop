using System.ComponentModel.DataAnnotations.Schema;

namespace ComicsShopMVC.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ComicBookId { get; set; }
        public int Amount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public virtual ComicBook ComicBook { get; set; } = default!;
        public virtual Order Order { get; set; } = default!;
    }
}