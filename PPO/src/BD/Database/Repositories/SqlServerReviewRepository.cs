using BusinesLogic;
using SqlServerDatabase.Repositories;
using System.Data.Linq;

namespace SqlServerDatabase
{
    public class SqlServerReviewRepository : SqlServerRepository, IReviewRepository
    {

        public SqlServerReviewRepository() : base() { }
        public void AddReview(Review review)
        {
            DataContext db = new DataContext(userConnection);
            Table<Reviews> reviewTable = db.GetTable<Reviews>();
            Reviews r = new Reviews()
            {
                GameId = review.GameId,
                UserLogin = review.UserLogin,
                Score = (int)review.Score,
                Text = review.Text,
                PublicationDate = review.PublicationDate
            };
            reviewTable.InsertOnSubmit(r);
            db.SubmitChanges();
        }

        public void DeleteReview(Review review)
        {
            DataContext db = new DataContext(adminConnection);
            Reviews rev = db.GetTable<Reviews>().Where(r => r.GameId == review.GameId && r.UserLogin == review.UserLogin).First();
            db.GetTable<Reviews>().DeleteOnSubmit(rev);
            db.SubmitChanges();
        }

        public List<Review> GetReviews(int id)
        {
            DataContext db = new DataContext(guestConnection);
            IQueryable<Reviews> gameReviews = from r in db.GetTable<Reviews>()
                                              where r.GameId == id
                                              select r;
            List<Review> reviews = new List<Review>();
            foreach (var rev in gameReviews)
                reviews.Add(new Review(rev.GameId, rev.UserLogin, (uint)rev.Score, rev.Text, rev.PublicationDate));
            return reviews;
        }
    }
}
