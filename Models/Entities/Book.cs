namespace TheBookClub.Models.Entities
{
    public class Book
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public Guid AuthorId { get; set; }
        public int Year { get; set; }
        public string Edition { get; set; }
        public string FileUrl { get; set; }
        public Guid GenreId { get; set; }
        public string Description { get; set; }
        public string? ISBN { get; set; }
        public string Language { get; set; }
        public string Publisher { get; set; }
        public string FileFormats { get; set; }
        public int DownloadCount { get; set; } = 0;
        public decimal? Price { get; set; }
        public string? PreviewUrl { get; set; }
        public string? CoverImage { get; set; }
        public decimal Rating { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<Orders> Orders { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual Author Author { get; set; }
        public virtual ICollection<Bookmark> Bookmark { get; set; }
        public virtual ICollection<BookPurchase> BookPurchase { get; set; }
        public virtual Genre Genre { get; set; }
    }
}