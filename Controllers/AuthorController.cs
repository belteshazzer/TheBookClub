using Microsoft.AspNetCore.Mvc;
using TheBookClub.Models.Dtos;
using TheBookClub.Services.AuthorService;
using RLIMS.Common;

namespace TheBookClub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet("get-authors")]	
        public async Task<IActionResult> GetAllAuthors()
        {
            var authors = await _authorService.GetAllAuthorsAsync();
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Data = authors,
                Message = "Authors retrieved successfully."
            });
        }

        [HttpGet("get-authors/{id}")]
        public async Task<IActionResult> GetAuthorById(Guid id)
        {
            var author = await _authorService.GetAuthorByIdAsync(id);
            if (author == null)
            {
                return NotFound(new ApiResponse
                {
                    StatusCode = 404,
                    Message = "Author not found."
                });
            }
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Data = author,
                Message = "Author retrieved successfully."
            });
        }

        [HttpPost("register-author")]
        public async Task<IActionResult> AddAuthor([FromBody] AuthorDto authorDto)
        {
            if (authorDto == null)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Invalid author data."
                });
            }

            var author = await _authorService.AddAuthorAsync(authorDto);
            return CreatedAtAction(nameof(GetAuthorById), new { id = author.Id }, new ApiResponse
            {
                StatusCode = 201,
                Data = author,
                Message = "Author added successfully."
            });
        }

        [HttpPut("/update-author/{id}")]
        public async Task<IActionResult> UpdateAuthor(Guid id, [FromBody] AuthorDto authorDto)
        {
            if (authorDto == null)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Invalid author data."
                });
            }

            var author = await _authorService.UpdateAuthorAsync(id, authorDto);
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Data = author,
                Message = "Author updated successfully."
            });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteAuthor(Guid id)
        {
            var result = await _authorService.DeleteAuthorAsync(id);
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = result ? "Author deleted successfully." : "Failed to delete author."
            });
        }

        [HttpPatch("delete/{id}")]
        public async Task<IActionResult> SoftDeleteAuthor(Guid id)
        {
            var result = await _authorService.SoftDeleteAuthorAsync(id);
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = result ? "Author soft deleted successfully." : "Failed to soft delete author."
            });
        }
    }
}