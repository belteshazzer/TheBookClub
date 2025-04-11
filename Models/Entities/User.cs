using Microsoft.AspNetCore.Identity;

namespace TheBookClub.Models.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SubscriptionType { get; set; } = "Free";
        public string? ProfilePicture { get; set; }
        public byte Status { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual ICollection<Orders> Orders { get; set; }
        public virtual ICollection<Bookmark> Bookmark { get; set; }
        public virtual ICollection<BookPurchase> BookPurchases { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}