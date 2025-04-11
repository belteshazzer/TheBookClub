using TheBookClub.Models.Dtos;
using TheBookClub.Models.Entities;

namespace TheBookClub.Services.ReviewService
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetAllReviewsAsync(Guid bookId);
        Task<Review> GetReviewByIdAsync(Guid id);
        Task<Review> AddReviewAsync(ReviewDto review);
        Task<Review> UpdateReviewAsync(Guid id, ReviewDto review);
        Task<bool> DeleteReviewAsync(Guid id);
        Task<bool> SoftDeleteReviewAsync(Guid id);
    }
}