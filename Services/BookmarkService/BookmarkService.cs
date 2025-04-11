using AutoMapper;
using TheBookClub.Models.Dtos;
using TheBookClub.Models.Entities;
using TheBookClub.Repositories;
using TheBookClub.Common;

namespace TheBookClub.Services.BookmarkService
{
    public class BookmarkService : IBookmarkService
    {
        private readonly IGenericRepository<Bookmark> _bookmarkRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BookmarkService(IGenericRepository<Bookmark> bookmarkRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _bookmarkRepository = bookmarkRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<Bookmark>> GetAllBookmarksAsync(Guid userId)
        {
            return await _bookmarkRepository.GetByConditionAsync(b => b.UserId == userId);
        }

        public async Task<Bookmark> GetBookmarkAsync(Guid userId, Guid bookId)
        {
            return await _bookmarkRepository.GetByConditionAsync(b => b.UserId == userId && b.BookId == bookId)
                                             .ContinueWith(t => t.Result.FirstOrDefault());
        }

        public async Task<Bookmark> AddBookmarkAsync(BookmarkDto bookmarkDto)
        {
            var userId = TokenHelper.GetUserId(_httpContextAccessor.HttpContext);

            var bookmark = _mapper.Map<Bookmark>(bookmarkDto);
            bookmark.UserId = userId;
            await _bookmarkRepository.AddAsync(bookmark);
            return bookmark;
        }

        public async Task<Bookmark> UpdateBookmarkAsync(Guid id, BookmarkDto bookmarkDto)
        {
            var bookmark = await _bookmarkRepository.GetByIdAsync(id);
            _mapper.Map(bookmarkDto, bookmark);
            await _bookmarkRepository.UpdateAsync(bookmark);
            return bookmark;
        }

        public async Task<bool> DeleteBookmarkAsync(Guid userId, Guid bookId)
        {
            var bookmark = await GetBookmarkAsync(userId, bookId);
            if (bookmark != null)
            {
                return await _bookmarkRepository.DeleteAsync(bookmark.Id);
            }
            return false;
        }

        public async Task<bool> SoftDeleteBookmarkAsync(Guid userId, Guid bookId)
        {
            var bookmark = await GetBookmarkAsync(userId, bookId);
            if (bookmark != null)
            {
                return await _bookmarkRepository.SoftDeleteAsync(bookmark.Id);
            }
            return false;
        }
    }
}