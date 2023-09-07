namespace BusinesLogic
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository reviewRepository;
        public ReviewService(IReviewRepository reviewRepository)
        {
            this.reviewRepository = reviewRepository;
        }
        public void DeleteReview(Review review)
        {
            reviewRepository.DeleteReview(review);
        }
        public void SaveReview(Review review)
        {
            reviewRepository.AddReview(review);
        }
        public List<Review> GetReviews(int gameId)
        {
            return reviewRepository.GetReviews(gameId);
        }
    }
}
