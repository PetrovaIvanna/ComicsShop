using System.ComponentModel.DataAnnotations;

namespace ComicsShopMVC.Models
{
    public class Publisher
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Publisher Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }

        [Range(1800, 2100, ErrorMessage = "Invalid Year")]
        [Display(Name = "Foundation Year")]
        public int FoundationYear { get; set; }
        public virtual ICollection<ComicBook>? ComicBooks { get; set; }
    }
}