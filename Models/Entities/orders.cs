namespace TheBookClub.Models.Entities
{
    public class Orders
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string DeliveryPlace { get; set; }
        public Guid? BookId { get; set; }
        public string? BookName { get; set; }
        public string? Author { get; set; }
        public string? Edition { get; set; }
        public byte OrderStatus { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;

        public virtual User User { get; set; }
        public virtual Book Book { get; set; }
    }
}