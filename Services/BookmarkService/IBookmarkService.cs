using TheBookClub.Models.Dtos;
using TheBookClub.Models.Entities;

namespace TheBookClub.Services.BookmarkService
{
    public interface IBookmarkService
    {
        Task<IEnumerable<Bookmark>> GetAllBookmarksAsync(Guid userId);
        Task<Bookmark> GetBookmarkAsync(Guid userId, Guid bookId);
        Task<Bookmark> AddBookmarkAsync(BookmarkDto bookmark);
        Task<Bookmark> UpdateBookmarkAsync(Guid id, BookmarkDto bookmark);
        Task<bool> DeleteBookmarkAsync(Guid userId, Guid bookId);
        Task<bool> SoftDeleteBookmarkAsync(Guid userId, Guid bookId);
    }
}