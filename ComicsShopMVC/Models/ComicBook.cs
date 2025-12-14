using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComicsShopMVC.Models
{
    public enum ComicCondition
    {
        New,
        Used,
        Damaged
    }

    public class ComicBook
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 chars")]
        public string Title { get; set; }

        [Required]
        [Range(1, 10000, ErrorMessage = "Issue number must be positive")]
        [Display(Name = "Issue #")]
        public int IssueNumber { get; set; }

        [Required]
        [Range(0.01, 10000.00, ErrorMessage = "Price must be positive")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Publication Date")]
        public DateTime PublicationDate { get; set; }

        [Display(Name = "Is Rare edition?")]
        public bool IsRare { get; set; }

        [Required]
        public ComicCondition Condition { get; set; }

        [Required(ErrorMessage = "Please select a publisher")]
        [Display(Name = "Publisher")]
        public int PublisherId { get; set; }
        public virtual Publisher? Publisher { get; set; }

        [Range(0, 100)]
        [Display(Name = "Discount (%)")]
        public int Discount { get; set; } = 0;

        [NotMapped]
        public decimal CurrentPrice
        {
            get
            {
                if (Discount == 0) return Price;
                return Math.Round(Price - (Price * (Discount / 100m)), 2);
            }
        }

        [NotMapped]
        public decimal Savings => Price - CurrentPrice;

        [Display(Name = "In Stock")]
        [Range(0, 1000)]
        public int QuantityInStock { get; set; }
    }
}