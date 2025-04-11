using AutoMapper;
using TheBookClub.Models.Dtos;
using TheBookClub.Models.Entities;
using TheBookClub.Repositories;

namespace TheBookClub.Services.BookPurchaseService
{
    public class BookPurchaseService : IBookPurchaseService
    {
        private readonly IGenericRepository<BookPurchase> _bookPurchaseRepository;
        private readonly IMapper _mapper;

        public BookPurchaseService(IGenericRepository<BookPurchase> bookPurchaseRepository, IMapper mapper)
        {
            _bookPurchaseRepository = bookPurchaseRepository;
            _mapper = mapper;
        }

        public async Task<BookPurchase> GetBookPurchaseByIdAsync(Guid id)
        {
            return await _bookPurchaseRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<BookPurchase>> GetAllBookPurchasesAsync()
        {
            return await _bookPurchaseRepository.GetAllAsync();
        }

        public async Task<BookPurchase> AddBookPurchaseAsync(BookPurchaseDto bookPurchaseDto)
        {
            var bookPurchase = _mapper.Map<BookPurchase>(bookPurchaseDto);
            await _bookPurchaseRepository.AddAsync(bookPurchase);
            return bookPurchase;
        }

        public async Task<BookPurchase> UpdateBookPurchaseAsync(Guid id, BookPurchaseDto bookPurchaseDto)
        {
            var bookPurchase = await _bookPurchaseRepository.GetByIdAsync(id);
            _mapper.Map(bookPurchaseDto, bookPurchase);
            await _bookPurchaseRepository.UpdateAsync(bookPurchase);
            return bookPurchase;
        }

        public async Task<bool> DeleteBookPurchaseAsync(Guid id)
        {
            return await _bookPurchaseRepository.DeleteAsync(id);
        }

        public async Task<bool> SoftDeleteBookPurchaseAsync(Guid id)
        {
            return await _bookPurchaseRepository.SoftDeleteAsync(id);
        }
    }
}