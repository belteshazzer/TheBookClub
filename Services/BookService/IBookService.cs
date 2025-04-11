using TheBookClub.Models.Dtos;
using TheBookClub.Models.Entities;

namespace TheBookClub.Services.BookService
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book> GetBookByIdAsync(Guid id);
        Task<Book> AddBookAsync(BookDto book);
        Task<Book> UpdateBookAsync(Guid id, BookDto book);
        Task<bool> DeleteBookAsync(Guid id);
        Task<bool> SoftDeleteBookAsync(Guid id);
    }
}