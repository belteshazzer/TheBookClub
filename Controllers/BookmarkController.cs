using Microsoft.AspNetCore.Mvc;
using TheBookClub.Models.Dtos;
using TheBookClub.Services.BookmarkService;
using RLIMS.Common;

namespace TheBookClub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookmarkController : ControllerBase
    {
        private readonly IBookmarkService _bookmarkService;

        public BookmarkController(IBookmarkService bookmarkService)
        {
            _bookmarkService = bookmarkService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAllBookmarks(Guid userId)
        {
            var bookmarks = await _bookmarkService.GetAllBookmarksAsync(userId);
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Data = bookmarks,
                Message = "Bookmarks retrieved successfully."
            });
        }

        [HttpGet("{userId}/{bookId}")]
        public async Task<IActionResult> GetBookmark(Guid userId, Guid bookId)
        {
            var bookmark = await _bookmarkService.GetBookmarkAsync(userId, bookId);
            if (bookmark == null)
            {
                return NotFound(new ApiResponse
                {
                    StatusCode = 404,
                    Message = "Bookmark not found."
                });
            }
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Data = bookmark,
                Message = "Bookmark retrieved successfully."
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddBookmark([FromBody] BookmarkDto bookmarkDto)
        {
            if (bookmarkDto == null)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Invalid bookmark data."
                });
            }

            var bookmark = await _bookmarkService.AddBookmarkAsync(bookmarkDto);
            return CreatedAtAction(nameof(GetBookmark), new { userId = bookmark.UserId, bookId = bookmark.BookId }, new ApiResponse
            {
                StatusCode = 201,
                Data = bookmark,
                Message = "Bookmark added successfully."
            });
        }

        [HttpDelete("{userId}/{bookId}")]
        public async Task<IActionResult> DeleteBookmark(Guid userId, Guid bookId)
        {
            var result = await _bookmarkService.DeleteBookmarkAsync(userId, bookId);
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = result ? "Bookmark deleted successfully." : "Failed to delete bookmark."
            });
        }

        [HttpPatch("{userId}/{bookId}")]
        public async Task<IActionResult> SoftDeleteBookmark(Guid userId, Guid bookId)
        {
            var result = await _bookmarkService.SoftDeleteBookmarkAsync(userId, bookId);
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = result ? "Bookmark soft deleted successfully." : "Failed to soft delete bookmark."
            });
        }
    }
}