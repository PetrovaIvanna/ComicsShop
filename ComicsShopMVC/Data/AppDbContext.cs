using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ComicsShopMVC.Models;

namespace ComicsShopMVC.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ComicBook> ComicBooks { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Publisher>().HasData(
                new Publisher { Id = 1, Name = "Marvel Comics", Country = "USA", FoundationYear = 1939 },
                new Publisher { Id = 2, Name = "DC Comics", Country = "USA", FoundationYear = 1935 },
                new Publisher { Id = 3, Name = "Dark Horse Comics", Country = "USA", FoundationYear = 1986 }
            );

            modelBuilder.Entity<ComicBook>().HasData(
                new ComicBook
                {
                    Id = 1,
                    Title = "Batman",
                    IssueNumber = 492,
                    Price = 15.00m,
                    PublicationDate = new DateTime(1993, 3, 18),
                    IsRare = true,
                    Condition = ComicCondition.Used,
                    PublisherId = 1,
                    QuantityInStock = 10
                },
                new ComicBook
                {
                    Id = 2,
                    Title = "Transformers: Lost Light",
                    IssueNumber = 24,
                    Price = 25.00m,
                    PublicationDate = new DateTime(2018, 9, 26),
                    IsRare = false,
                    Condition = ComicCondition.Used,
                    PublisherId = 3,
                    QuantityInStock = 3
                },
                new ComicBook
                {
                    Id = 3,
                    Title = "The Amazing Spider-Man",
                    IssueNumber = 19,
                    Price = 19.00m,
                    PublicationDate = new DateTime(2025, 5, 28),
                    IsRare = false,
                    Condition = ComicCondition.New,
                    PublisherId = 1,
                    QuantityInStock = 100
                },
                new ComicBook
                {
                    Id = 4,
                    Title = "The Amazing Spider-Man",
                    IssueNumber = 20,
                    Price = 18.99m,
                    PublicationDate = new DateTime(2025, 6, 28),
                    IsRare = false,
                    Condition = ComicCondition.New,
                    PublisherId = 1,
                    QuantityInStock = 100
                },
                new ComicBook
                {
                    Id = 5,
                    Title = "The Amazing Spider-Man",
                    IssueNumber = 20,
                    Price = 12.99m,
                    PublicationDate = new DateTime(2025, 6, 28),
                    IsRare = false,
                    Condition = ComicCondition.Used,
                    PublisherId = 1,
                    QuantityInStock = 1
                }
            );
        }
    }
}