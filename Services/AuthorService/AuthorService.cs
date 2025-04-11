using AutoMapper;
using TheBookClub.Models.Dtos;
using TheBookClub.Models.Entities;
using TheBookClub.Repositories;

namespace TheBookClub.Services.AuthorService
{
    public class AuthorService : IAuthorService
    {
        private readonly IGenericRepository<Author> _authorRepository;
        private readonly IMapper _mapper;

        public AuthorService(IGenericRepository<Author> authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Author>> GetAllAuthorsAsync()
        {
            return await _authorRepository.GetAllAsync();
        }

        public async Task<Author> GetAuthorByIdAsync(Guid id)
        {
            return await _authorRepository.GetByIdAsync(id);
        }

        public async Task<Author> AddAuthorAsync(AuthorDto authorDto)
        {
            var author = _mapper.Map<Author>(authorDto);
            await _authorRepository.AddAsync(author);
            return author;
        }

        public async Task<Author> UpdateAuthorAsync(Guid id, AuthorDto authorDto)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            _mapper.Map(authorDto, author);
            await _authorRepository.UpdateAsync(author);
            return author;
        }

        public async Task<bool> DeleteAuthorAsync(Guid id)
        {
            return await _authorRepository.DeleteAsync(id);
        }

        public async Task<bool> SoftDeleteAuthorAsync(Guid id)
        {
            return await _authorRepository.SoftDeleteAsync(id);
        }
    }
}