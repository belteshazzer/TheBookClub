namespace TheBookClub.Models.Entities
{
    public class Bookmark
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
        public Guid GenreId { get; set; } 
        public int? PageNumber { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;

        public virtual User User { get; set; }
        public virtual Book Book { get; set; }
    }
}