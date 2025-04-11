using TheBookClub.Models.Dtos;
using TheBookClub.Models.Entities;

namespace TheBookClub.Services.GenreService
{
    public interface IGenreService
    {
        Task<IEnumerable<Genre>> GetAllGenresAsync();
        Task<Genre> GetGenreByIdAsync(Guid genreId);
        Task<Genre> CreateGenreAsync(GenreDto genreDto);
        Task<Genre> UpdateGenreAsync(Guid genreId, GenreDto genreDto);
        Task<bool> DeleteGenreAsync(Guid genreId);
        Task<bool> SoftDeleteGenreAsync(Guid genreId);
    }
}