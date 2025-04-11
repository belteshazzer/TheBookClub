using AutoMapper;
using TheBookClub.Models.Dtos;
using TheBookClub.Models.Entities;
using TheBookClub.Repositories;

namespace TheBookClub.Services.GenreService
{
    public class GenreService : IGenreService
    {
        private readonly IGenericRepository<Genre> _genreRepository;
        private readonly IMapper _mapper;

        public GenreService(IGenericRepository<Genre> genreRepository, IMapper mapper)
        {
            _genreRepository = genreRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Genre>> GetAllGenresAsync()
        {
            return await _genreRepository.GetAllAsync();
        }

        public async Task<Genre> GetGenreByIdAsync(Guid genreId)
        {
            return await _genreRepository.GetByIdAsync(genreId);
        }

        public async Task<Genre> CreateGenreAsync(GenreDto genreDto)
        {
            var genre = _mapper.Map<Genre>(genreDto);
            await _genreRepository.AddAsync(genre);
            return genre;
        }

        public async Task<Genre> UpdateGenreAsync(Guid genreId, GenreDto genreDto)
        {
            var genre = await _genreRepository.GetByIdAsync(genreId);
            _mapper.Map(genreDto, genre);
            await _genreRepository.UpdateAsync(genre);
            return genre;
        }

        public async Task<bool> DeleteGenreAsync(Guid genreId)
        {
            return await _genreRepository.DeleteAsync(genreId);
        }

        public async Task<bool> SoftDeleteGenreAsync(Guid genreId)
        {
            return await _genreRepository.SoftDeleteAsync(genreId);
        }
    }
}