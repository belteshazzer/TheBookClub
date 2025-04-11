using TheBookClub.Models.Dtos;
using TheBookClub.Models.Entities;

namespace TheBookClub.Services.BookPurchaseService
{
    public interface IBookPurchaseService
    {
        Task<BookPurchase> GetBookPurchaseByIdAsync(Guid id);
        Task<IEnumerable<BookPurchase>> GetAllBookPurchasesAsync();
        Task<BookPurchase> AddBookPurchaseAsync(BookPurchaseDto bookPurchase);
        Task<BookPurchase> UpdateBookPurchaseAsync(Guid id, BookPurchaseDto bookPurchase);
        Task<bool> DeleteBookPurchaseAsync(Guid id);
        Task<bool> SoftDeleteBookPurchaseAsync(Guid id);
    }
}