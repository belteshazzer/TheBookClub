namespace TheBookClub.Models.Dtos
{
    public class BookDto
    {
        public string Name { get; set; }
        public Guid AuthorId { get; set; }
        public int Year { get; set; }
        public string? Edition { get; set; }
        public string FileUrl { get; set; }
        public Guid? GenreId { get; set; }
        public string? Description { get; set; }
        public string? ISBN { get; set; }
        public string Language { get; set; }
        public string Publisher { get; set; }
        public string FileFormats { get; set; }
        public int DownloadCount { get; set; } = 0;
        public decimal? Price { get; set; }
        public string? PreviewUrl { get; set; }
        public string? CoverImage { get; set; }
    }
}