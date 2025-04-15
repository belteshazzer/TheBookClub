namespace TheBookClub.Models.Entities
{
    public class Notification
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
        public bool IsDeleted { get; set; } = false;

        public virtual User User { get; set; }
    }
}