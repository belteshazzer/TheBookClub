using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TheBookClub.Context;
using TheBookClub.Models.Dtos;
using TheBookClub.Models.Entities;
using TheBookClub.Repositories;

namespace TheBookClub.Services.ReviewService
{
    public class ReviewService : IReviewService
    {
        private readonly IGenericRepository<Review> _reviewRepository;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;
        public ReviewService(IGenericRepository<Review> reviewRepository, IMapper mapper, ApplicationDbContext dbContext)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _dbContext = dbContext;
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
            await UpdateBookRatingAsync(review.BookId);
            return review;
        }

        public async Task<Review> UpdateReviewAsync(Guid id, ReviewDto reviewDto)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            _mapper.Map(reviewDto, review);
            await _reviewRepository.UpdateAsync(review);
            await UpdateBookRatingAsync(review.BookId);
            return review;
        }

        public async Task<bool> DeleteReviewAsync(Guid id)
        {
            var result = await _reviewRepository.DeleteAsync(id);
            if (result)
            {
                await UpdateBookRatingAsync(id);
            }
            return result;

        }

        public async Task<bool> SoftDeleteReviewAsync(Guid id)
        {
            return await _reviewRepository.SoftDeleteAsync(id);
        }


        public async Task UpdateBookRatingAsync(Guid bookId)
        {
            var averageRating = await _dbContext.Reviews
                .Where(r => r.BookId == bookId)
                .AverageAsync(r => (float?)r.Rating) ?? 0;

            var book = await _dbContext.Books.FindAsync(bookId);
            if (book != null)
            {
                book.Rating = (decimal)averageRating;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}