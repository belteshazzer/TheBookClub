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
            if (bookDto.Upload != null)
            {                
                var uploadsFolder = "C:\\Users\\CBE\\Projects\\personal\\TheBookClub\\Uploads";
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                var fileName = $"{Guid.NewGuid()}_{bookDto.Upload.FileName}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await bookDto.Upload.CopyToAsync(stream);
                }
                book.FileUrl = filePath;

                await _bookRepository.AddAsync(book);
            }

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
            var book = await _bookRepository.GetByIdAsync(id);
            if (!string.IsNullOrEmpty(book.FileUrl) && File.Exists(book.FileUrl))
            {
                File.Delete(book.FileUrl);
            }
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

        public async Task<(Stream FileStream, string FileName)> GetBookFileAsync(Guid id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if(book == null || string.IsNullOrEmpty(book.FileUrl))
            {
                throw new FileNotFoundException("Book not found.");
            }

            var filePath = book.FileUrl;
            var fileName = Path.GetFileName(filePath);

            if(!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found.");
            }

            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return (fileStream, fileName);
        }

        public async Task DownloadBookAsync(Guid id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book != null)
            {
                book.DownloadCount++;
                await _bookRepository.UpdateAsync(book);
            }
            else
            {
                throw new Exception("Book not found.");
            }
        }
    }
}