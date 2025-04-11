using AutoMapper;
using TheBookClub.Models.Dtos;
using TheBookClub.Models.Entities;
using TheBookClub.Repositories;

namespace TheBookClub.Services.BookService
{
    public class BookService : IBookService
    {
        private readonly IGenericRepository<Book> _bookRepository;
        private readonly IMapper _mapper;

        public BookService(IGenericRepository<Book> bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
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
    }
}