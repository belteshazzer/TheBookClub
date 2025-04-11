using AutoMapper;
using TheBookClub.Models.Dtos;
using TheBookClub.Models.Entities;
using TheBookClub.Repositories;

namespace TheBookClub.Services.ReviewService
{
    public class ReviewService : IReviewService
    {
        private readonly IGenericRepository<Review> _reviewRepository;
        private readonly IMapper _mapper;

        public ReviewService(IGenericRepository<Review> reviewRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Review>> GetAllReviewsAsync(Guid bookId)
        {
            return await _reviewRepository.GetByConditionAsync(r => r.BookId == bookId);
        }

        public async Task<Review> GetReviewByIdAsync(Guid id)
        {
            return await _reviewRepository.GetByIdAsync(id);
        }

        public async Task<Review> AddReviewAsync(ReviewDto reviewDto)
        {
            var review = _mapper.Map<Review>(reviewDto);
            await _reviewRepository.AddAsync(review);
            return review;
        }

        public async Task<Review> UpdateReviewAsync(Guid id, ReviewDto reviewDto)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            _mapper.Map(reviewDto, review);
            await _reviewRepository.UpdateAsync(review);
            return review;
        }

        public async Task<bool> DeleteReviewAsync(Guid id)
        {
            return await _reviewRepository.DeleteAsync(id);
        }

        public async Task<bool> SoftDeleteReviewAsync(Guid id)
        {
            return await _reviewRepository.SoftDeleteAsync(id);
        }
    }
}