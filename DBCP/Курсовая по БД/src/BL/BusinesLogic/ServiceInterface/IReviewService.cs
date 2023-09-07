namespace BusinesLogic
{
    public interface IReviewService
    {
        public void SaveReview(Review review);
        public void DeleteReview(Review review);
        public List<Review> GetReviews(int gameId);
    }
}
