using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheBookClub.Models.Entities;

namespace TheBookClub.Context
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSet properties for each entity
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<BookPurchase> BookPurchases { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())"); 
                entity.Property(e => e.Status).HasDefaultValue((byte)1); // Default status to 1 (Active)
                entity.Property(e => e.UserName).HasComputedColumnSql("[Email]"); 
                entity.Property(e => e.SubscriptionType).HasDefaultValue("Free"); 
            });

            modelBuilder.Entity<IdentityRole<Guid>>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

            });

            // Configure relationships and constraints if necessary
            modelBuilder.Entity<Author>(e =>
            {
                e.HasKey(a => a.Id);
                e.Property(a => a.Id).HasDefaultValueSql("(newid())"); 
            });
                

            modelBuilder.Entity<Bookmark>(e => 
            {
                e.HasKey(b => b.Id);
                e.Property(b => b.Id).HasDefaultValueSql("(newid())");
                e.HasOne(b => b.User)
                    .WithMany(u => u.Bookmark)
                    .HasForeignKey(b => b.UserId)
                    .OnDelete(DeleteBehavior.Cascade); 
                e.HasOne(b => b.Book)
                    .WithMany()
                    .HasForeignKey(b => b.BookId)
                    .OnDelete(DeleteBehavior.Cascade); 
            });

            modelBuilder.Entity<Orders>(e =>
            {
                e.HasKey(o => o.Id);
                e.Property(o => o.Id).HasDefaultValueSql("(newid())");
                e.Property(o => o.OrderDate).IsRequired().HasDefaultValueSql("getdate()"); 
                e.HasOne(o => o.User)
                    .WithMany(u => u.Orders)
                    .HasForeignKey(o => o.UserId)
                    .OnDelete(DeleteBehavior.Cascade); 
                e.HasOne(o => o.Book)
                    .WithMany() 
                    .HasForeignKey(o => o.BookId)
                    .OnDelete(DeleteBehavior.Cascade); 
                e.Property(e => e.OrderStatus).HasDefaultValue((byte)0); // pending

            });

            modelBuilder.Entity<Book>(e =>
            {
                e.HasKey(b => b.Id);
                e.Property(b => b.Id).HasDefaultValueSql("(newid())");
                e.HasOne(b => b.Author)
                    .WithMany(a => a.Books)
                    .HasForeignKey(b => b.AuthorId)
                    .OnDelete(DeleteBehavior.Cascade); 
                e.HasOne(b => b.Genre)
                    .WithMany(g => g.Books)
                    .HasForeignKey(b => b.GenreId)
                    .OnDelete(DeleteBehavior.Cascade); 
                e.Property(e => e.Rating).HasDefaultValue(0);
            });

            modelBuilder.Entity<BookPurchase>(e =>
            {
                e.HasKey(bp => bp.Id);
                e.Property(bp => bp.Id).HasDefaultValueSql("(newid())");
                e.Property(bp => bp.PurchaseDate).IsRequired().HasDefaultValueSql("getdate()"); 
                
                e.HasOne(bp => bp.Seller)
                    .WithMany(u => u.BookPurchases)
                    .HasForeignKey(bp => bp.SellerId)
                    .OnDelete(DeleteBehavior.Cascade); 
                e.HasOne(bp => bp.Book)
                    .WithMany(b => b.BookPurchase)
                    .HasForeignKey(bp => bp.BookId)
                    .OnDelete(DeleteBehavior.Cascade); 
            });

            modelBuilder.Entity<Review>(e =>
            {
                e.HasKey(r => r.Id);
                e.Property(r => r.Id).HasDefaultValueSql("(newid())");
                e.Property(r => r.ReviewDate).IsRequired().HasDefaultValueSql("getdate()"); 
                
                e.HasOne(r => r.User)
                    .WithMany(u => u.Reviews)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Cascade); 
                e.HasOne(r => r.Book)
                    .WithMany()
                    .HasForeignKey(r => r.BookId)
                    .OnDelete(DeleteBehavior.Cascade); 
                e.Property(e => e.ReviewDate).HasDefaultValueSql("getdate()"); 
            });

            modelBuilder.Entity<Genre>(e =>
            {
                e.HasKey(g => g.Id);
                e.Property(g => g.Id).HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<Notification>(e =>
            {
                e.HasKey(n => n.Id);
                e.Property(n => n.Id).HasDefaultValueSql("(newid())");
                e.Property(n => n.CreatedAt).IsRequired().HasDefaultValueSql("getdate()"); 
                e.HasOne(n => n.User)
                    .WithMany()
                    .HasForeignKey(n => n.UserId)
                    .OnDelete(DeleteBehavior.Cascade); 
            });
        }
    }
}