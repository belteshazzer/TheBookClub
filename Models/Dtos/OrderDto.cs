namespace TheBookClub.Models.Dtos
{
    public class OrderDto
    {
        public Guid UserId { get; set; }
        public string DeliveryPlace { get; set; }
        public Guid? BookId { get; set; }
        public string? BookName { get; set; }
        public string? Author { get; set; }
        public string? Edition { get; set; }
        public byte OrderStatus { get; set; }
    }
}