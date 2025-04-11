using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RLIMS.Common;
using TheBookClub.Models.Dtos;
using TheBookClub.Services.BookService;

namespace TheBookClub.Controllers{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("get-books")]	
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(new ApiResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = books,
                Message = "Books retrieved successfully."
            });
        }

        [HttpGet("get-book/{id}")]
        public async Task<IActionResult> GetBookById(Guid id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound(new ApiResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "Book not found."
                });
            }
            return Ok(new ApiResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = book,
                Message = "Book retrieved successfully."
            });
        }

        [HttpPost("add-book")]
        public async Task<IActionResult> AddBook([FromBody] BookDto bookDto)
        {
            if (bookDto == null)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid book data."
                });
            }

            var book = await _bookService.AddBookAsync(bookDto);
            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, new ApiResponse
            {
                StatusCode = StatusCodes.Status201Created,
                Data = book,
                Message = "Book added successfully."
            });
        }

        [HttpPut("update-book/{id}")]
        public async Task<IActionResult> UpdateBook(Guid id, [FromBody] BookDto bookDto)
        {
            if (bookDto == null)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Invalid book data."
                });
            }

            var book = await _bookService.UpdateBookAsync(id, bookDto);
            return Ok(new ApiResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Data = book,
                Message = "Book updated successfully."
            });
        }

        [HttpDelete("delete-book/{id}")]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            var result = await _bookService.DeleteBookAsync(id);

            return Ok(new ApiResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Book deleted successfully."
            });
        }

        [HttpPatch("delete-book/{id}")]
        public async Task<IActionResult> SoftDeleteBook(Guid id)
        {
            var result = await _bookService.SoftDeleteBookAsync(id);

            return Ok(new ApiResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Message = "Book soft deleted successfully."
            });
        }
    }
}