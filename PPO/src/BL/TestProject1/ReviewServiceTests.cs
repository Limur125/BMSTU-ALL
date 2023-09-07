namespace TestProject1
{
    [TestClass]
    public class ReviewServiceTests
    {
        [TestMethod]
        public void SaveReviewTest()
        {
            ReviewRepositoryStub reviewRepository = new ReviewRepositoryStub(new List<Review>());
            IReviewService reviewService = new ReviewService(reviewRepository);
            Review rev = new Review(1, "User1", 10, "tetettext", new DateTime(2022, 8, 23));

            reviewService.SaveReview(rev);

            List<Review> actList = reviewRepository.reviews;
            Assert.AreEqual(1, actList.Count);
            Review actual = actList[0];
            Assert.AreEqual("User1", actual.UserLogin);
            Assert.AreEqual(10u, actual.Score);
            Assert.AreEqual("tetettext", actual.Text);
            Assert.AreEqual(new DateTime(2022, 8, 23), actual.PublicationDate);
        }
        [TestMethod]
        public void DeleteReviewTest()
        {
            Review rev = new Review(1, "User1", 10, "tetettext", new DateTime(2022, 8, 23));

            List<Review> reviews = new List<Review>()
            {
                rev,
                new Review(1, "User5", 5, "text", new DateTime(2022, 4, 1))
            };
            ReviewRepositoryStub reviewRepository = new ReviewRepositoryStub(reviews);
            IReviewService reviewService = new ReviewService(reviewRepository);

            reviewService.DeleteReview(rev);

            List<Review> actList = reviewRepository.reviews;
            Assert.AreEqual(1, actList.Count);
            Review actual = actList[0];
            Assert.AreEqual("User5", actual.UserLogin);
            Assert.AreEqual(5u, actual.Score);
            Assert.AreEqual("text", actual.Text);
            Assert.AreEqual(new DateTime(2022, 4, 1), actual.PublicationDate);
        }

        [TestMethod]
        public void GetReviewsTest()
        {
            List<Review> reviews = new List<Review>()
            {
                new Review(1, "User1", 10, "tetettext", new DateTime(2022, 8, 23)),
                new Review(1, "User5", 5, "text", new DateTime(2022, 4, 1))
            };
            ReviewRepositoryStub reviewRepository = new ReviewRepositoryStub(reviews);
            IReviewService reviewService = new ReviewService(reviewRepository);

            List<Review> actList = reviewService.GetReviews(1);

            Assert.AreEqual(2, actList.Count);
            Review actual = actList[0];
            Assert.AreEqual("User1", actual.UserLogin);
            Assert.AreEqual(10u, actual.Score);
            Assert.AreEqual("tetettext", actual.Text);
            Assert.AreEqual(new DateTime(2022, 8, 23), actual.PublicationDate);
            actual = actList[1];
            Assert.AreEqual("User5", actual.UserLogin);
            Assert.AreEqual(5u, actual.Score);
            Assert.AreEqual("text", actual.Text);
            Assert.AreEqual(new DateTime(2022, 4, 1), actual.PublicationDate);
        }
    }
}
