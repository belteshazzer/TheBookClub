namespace TheBookClub.Models.Entities
{
    public class Review 
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime ReviewDate { get; set; }
        public bool IsDeleted { get; set; } = false;

        public virtual User User { get; set; }
        public virtual Book Book { get; set; }
    }
}