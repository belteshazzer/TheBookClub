namespace TheBookClub.Models.Dtos
{
    public class BookPurchaseDto
    {
        public Guid SellerId { get; set; }
        public Guid BookId { get; set; }
        public decimal Price { get; set; }
        public string TransactionStatus { get; set; }
    }
}