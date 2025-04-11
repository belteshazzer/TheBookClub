using Microsoft.AspNetCore.Mvc;
using TheBookClub.Models.Dtos;
using TheBookClub.Services.ReviewService;
using RLIMS.Common;

namespace TheBookClub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("{bookId}")]
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

        [HttpGet("{id}")]
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