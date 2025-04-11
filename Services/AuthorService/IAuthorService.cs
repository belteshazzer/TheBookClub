using TheBookClub.Models.Dtos;
using TheBookClub.Models.Entities;

namespace TheBookClub.Services.AuthorService
{
    public interface IAuthorService
    {
        Task<IEnumerable<Author>> GetAllAuthorsAsync();
        Task<Author> GetAuthorByIdAsync(Guid id);
        Task<Author> AddAuthorAsync(AuthorDto author);
        Task<Author> UpdateAuthorAsync(Guid id, AuthorDto author);
        Task<bool> DeleteAuthorAsync(Guid id);
        Task<bool> SoftDeleteAuthorAsync(Guid id);
    }
}