namespace TestProject1
{
    internal class ReviewRepositoryStub : IReviewRepository
    {
        public List<Review> reviews;
        public ReviewRepositoryStub(List<Review> reviews)
        {
            this.reviews = reviews;
        }
        public void AddReview(Review review)
        {
            if (reviews.Find(r => r.UserLogin == review.UserLogin && r.GameId == review.GameId) != null)
                throw new Exception($"{review.UserLogin} already left review for {review.GameId}");
            reviews.Add(review);
        }

        public void DeleteReview(Review review)
        {
            if (reviews.RemoveAll(r => r.UserLogin == review.UserLogin && r.GameId == review.GameId) == 0)
                throw new Exception($"Reviews not found for {review.UserLogin} {review.GameId}");
        }

        public List<Review> GetReviews(int id)
        {
            return reviews.FindAll(r => r.GameId == id);
        }
    }
}
