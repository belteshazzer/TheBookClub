namespace TheBookClub.Models.Entities
{
    public class BookPurchase
    {
        public Guid Id { get; set; }
        public Guid SellerId { get; set; }
        public Guid BookId { get; set; }
        public decimal Price { get; set; }
        public string TransactionStatus { get; set; }
        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;

        public virtual User Seller { get; set; }
        public virtual Book Book { get; set; }
    }
}