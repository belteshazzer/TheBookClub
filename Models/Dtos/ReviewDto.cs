namespace TheBookClub.Models.Dtos
{
    public class ReviewDto
    {
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}