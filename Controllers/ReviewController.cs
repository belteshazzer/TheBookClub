using Microsoft.AspNetCore.Mvc;
using TheBookClub.Models.Dtos;
using TheBookClub.Services.ReviewService;
using TheBookClub.Common;
using Microsoft.AspNetCore.Authorization;

namespace TheBookClub.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("get-reviews/{bookId}")]
        public async Task<IActionResult> GetAllReviews(Guid bookId)
        {
            var reviews = await _reviewService.GetAllReviewsAsync(bookId);
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Data = reviews,
                Message = "Reviews retrieved successfully."
            });
        }

        [HttpGet("get-review/{id}")]
        public async Task<IActionResult> GetReviewById(Guid id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Data = review,
                Message = "Review retrieved successfully."
            });
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost("add-review")]	
        public async Task<IActionResult> AddReview([FromBody] ReviewDto reviewDto)
        {
            if (reviewDto == null)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Invalid review data."
                });
            }

            var review = await _reviewService.AddReviewAsync(reviewDto);
            return CreatedAtAction(nameof(GetReviewById), new { id = review.Id }, new ApiResponse
            {
                StatusCode = 201,
                Data = review,
                Message = "Review added successfully."
            });
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(Guid id, [FromBody] ReviewDto reviewDto)
        {
            if (reviewDto == null)
            {
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = "Invalid review data."
                });
            }

            var review = await _reviewService.UpdateReviewAsync(id, reviewDto);
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Data = review,
                Message = "Review updated successfully."
            });
        }

        [Authorize(Roles = "Admin, User")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(Guid id)
        {
            var result = await _reviewService.DeleteReviewAsync(id);
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = result ? "Review deleted successfully." : "Failed to delete review."
            });
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> SoftDeleteReview(Guid id)
        {
            var result = await _reviewService.SoftDeleteReviewAsync(id);
            return Ok(new ApiResponse
            {
                StatusCode = 200,
                Message = result ? "Review soft deleted successfully." : "Failed to soft delete review."
            });
        }
    }
}