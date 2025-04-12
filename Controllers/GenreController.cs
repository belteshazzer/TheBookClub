using Microsoft.AspNetCore.Mvc;
using TheBookClub.Models.Dtos;
using TheBookClub.Services.GenreService;
using RLIMS.Common;
using Microsoft.AspNetCore.Authorization;

namespace TheBookClub.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("get-genres")]	
        public async Task<IActionResult> GetAllGenres()
        {
            var genres = await _genreService.GetAllGenresAsync();
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Data = genres,
                Message = "Genres retrieved successfully."
            });
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("get-genre/{id}")]
        public async Task<IActionResult> GetGenreById(Guid id)
        {
            var genre = await _genreService.GetGenreByIdAsync(id);
    
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Data = genre,
                Message = "Genre retrieved successfully."
            });
        }

        [HttpPost("add-genre")]	
        public async Task<IActionResult> AddGenre([FromBody] GenreDto genreDto)
        {
            if (genreDto == null)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Invalid genre data."
                });
            }

            var genre = await _genreService.CreateGenreAsync(genreDto);
            return CreatedAtAction(nameof(GetGenreById), new { id = genre.Id }, new ApiResponse
            {
                StatusCode = 201,
                Data = genre,
                Message = "Genre added successfully."
            });
        }

        [HttpPut("update-genre/{id}")]
        public async Task<IActionResult> UpdateGenre(Guid id, [FromBody] GenreDto genreDto)
        {
            if (genreDto == null)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Invalid genre data."
                });
            }

            var genre = await _genreService.UpdateGenreAsync(id, genreDto);
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Data = genre,
                Message = "Genre updated successfully."
            });
        }

        [HttpDelete("delete-genre/{id}")]
        public async Task<IActionResult> DeleteGenre(Guid id)
        {
            var result = await _genreService.DeleteGenreAsync(id);
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = result ? "Genre deleted successfully." : "Failed to delete genre."
            });
        }

        [HttpPatch("delete-genre/{id}")]
        public async Task<IActionResult> SoftDeleteGenre(Guid id)
        {
            var result = await _genreService.SoftDeleteGenreAsync(id);
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = result ? "Genre soft deleted successfully." : "Failed to soft delete genre."
            });
        }
    }
}