using AutoMapper;
using TheBookClub.Models.Dtos;
using TheBookClub.Models.Entities;
using TheBookClub.Repositories;
using TheBookClub.Services.NotificationService;

namespace TheBookClub.Services.BookService
{
    public class BookService : IBookService
    {
        private readonly IGenericRepository<Book> _bookRepository;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Bookmark> _bookmarkRepository;
        private readonly INotificationService _notificationService;

        public BookService(IGenericRepository<Book> bookRepository, IGenericRepository<Bookmark> bookmarkRepository, IMapper mapper, INotificationService notificationService)
        {
            _bookRepository = bookRepository;
            _bookmarkRepository = bookmarkRepository;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _bookRepository.GetAllAsync();
        }

        public async Task<Book> GetBookByIdAsync(Guid id)
        {
            return await _bookRepository.GetByIdAsync(id);
        }

        public async Task<Book> AddBookAsync(BookDto bookDto)
        {
            var book = _mapper.Map<Book>(bookDto);
            await _bookRepository.AddAsync(book);

            await AnnounceBookAddedAsync(book.GenreId, $"New book added: {book.Name} by {book.Author}");
            return book;
        }

        public async Task<Book> UpdateBookAsync(Guid id, BookDto bookDto)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            _mapper.Map(bookDto, book);
            await _bookRepository.UpdateAsync(book);

            return book;
        }
  
        public async Task<bool> DeleteBookAsync(Guid id)
        {
            return await _bookRepository.DeleteAsync(id);
        }

        public async Task<bool> SoftDeleteBookAsync(Guid id)
        {
            return await _bookRepository.SoftDeleteAsync(id);
        }

        public async Task AnnounceBookAddedAsync(Guid genreId, string message)
        {
            var targetUsers = (await _bookmarkRepository.GetByConditionAsync(b => b.GenreId == genreId)).Select(b => b.UserId).ToList();

            await _notificationService.SendGroupNotificationAsync(targetUsers, message);
        }
    }
}