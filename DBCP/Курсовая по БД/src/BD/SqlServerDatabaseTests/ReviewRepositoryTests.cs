using System.Data.Linq;

namespace SqlServerDatabaseTests
{
    [TestClass]
    public class ReviewRepositoryTests
    {
        private readonly string connection = "Data Source=192.168.233.189,1433;Database=GameTime;User ID=SA;Password=Password_1;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        [TestMethod]
        public void AddTimeRecordNormalTest()
        {
            DataContext db = new DataContext(connection);
            int expectedCount = db.GetTable<Reviews>().Count() + 1;
            SqlServerReviewRepository ssrr = new SqlServerReviewRepository();
            Review rev = new Review(0, "User1", 10, "text", DateTime.Today);

            ssrr.AddReview(rev);

            Table<Reviews> rTable = db.GetTable<Reviews>();
            db.Refresh(RefreshMode.OverwriteCurrentValues, rTable);
            Reviews r = rTable.Single(r => r.GameId == 0 && r.UserLogin == "User1");
            Assert.AreEqual(expectedCount, rTable.Count());
            Assert.IsNotNull(r);
            Assert.AreEqual(0, r.GameId);
            Assert.AreEqual("User1", r.UserLogin);
            rTable.DeleteOnSubmit(r);
            db.SubmitChanges();
        }
        [TestMethod]
        public void GetTimeRecordNormalTest()
        {
            DataContext db = new DataContext(connection);
            SqlServerReviewRepository ssrr = new SqlServerReviewRepository();
            Reviews rec = new Reviews() { GameId = 0, UserLogin = "User1", Score = 10, Text = "text", PublicationDate = DateTime.Today };
            Table<Reviews> rTable = db.GetTable<Reviews>();
            rTable.InsertOnSubmit(rec);
            db.SubmitChanges();

            List<Review> recs = ssrr.GetReviews(0);


            Assert.AreEqual(1, recs.Count);
            foreach (Review r in recs)
                Assert.AreEqual(0, r.GameId);
            rTable.DeleteOnSubmit(rec);
            db.SubmitChanges();
        }
        [TestMethod]
        public void DeleteGameNormalTest()
        {
            DataContext db = new DataContext(connection);
            Table<Reviews> rTable = db.GetTable<Reviews>();
            SqlServerReviewRepository ssgr = new SqlServerReviewRepository();
            Reviews inr = new Reviews() { GameId = 0, UserLogin = "User1", Score = 10, Text = "text", PublicationDate = DateTime.Today };
            rTable.InsertOnSubmit(inr);
            db.SubmitChanges();
            int expectedCount = db.GetTable<Reviews>().Count() - 1;
            Review rev = new Review(0, "User1", 10, "text", DateTime.Today);

            ssgr.DeleteReview(rev);

            db.Refresh(RefreshMode.OverwriteCurrentValues, rTable);
            rTable = db.GetTable<Reviews>();
            Assert.AreEqual(0, rTable.Where(r => r.GameId == 0).Count());
            Assert.AreEqual(expectedCount, rTable.Count());
        }
    }
}
