namespace BusinesLogic
{
    public interface IReviewRepository
    {
        public void AddReview(Review review);
        public List<Review> GetReviews(int id);
        public void DeleteReview(Review review);
    }
}
