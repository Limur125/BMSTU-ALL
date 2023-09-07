namespace BusinesLogic
{
    public class GameService : IGameService
    {
        private readonly IGameRepository gameRepository;
        private readonly IReviewRepository reviewRepository;
        private readonly ITimeRecordRepository timeRecordRepository;
        public GameService(IGameRepository gameRepository, IReviewRepository reviewRepository, ITimeRecordRepository timeRecordRepository)
        {
            this.gameRepository = gameRepository;
            this.reviewRepository = reviewRepository;
            this.timeRecordRepository = timeRecordRepository;
        }
        public void AddGame(Game game)
        {
            gameRepository.AddGame(game);
        }

        public void AddGameXMLApp(Game game)
        {
            gameRepository.AddGameXMLApp(game);
        }
        public void AddGameXMLSQL(Game game)
        {
            gameRepository.AddGameXMLApp(game);
        }

        public void DeleteGame(string title)
        {
            gameRepository.DeleteGame(title);
        }

        public GameInfo GetGame(string title)
        {
            Game g = gameRepository.GetGame(title);
            if (g == null)
                throw new GameNotFoundException("Game not found");
            List<Review> reviews = reviewRepository.GetReviews(g.Id);
            List<TimeRecord> timeRecords = timeRecordRepository.GetGameTimeRecords(g.Id);
            (TimeInfo normal, TimeInfo full) = GetTimeInfo(timeRecords);
            decimal avgScore = AverageScore(reviews);
            List<Review> mostReviews = SelectMostReviews(reviews, 2);
            GameInfo gf = new GameInfo(g, avgScore, normal, full, mostReviews);
            return gf;
        }

        public List<string> GetLibrary()
        {
            List<Game> games = gameRepository.GetAllGames();
            IEnumerable<string> library = from game in games
                                          select game.Title;
            return library.ToList();
        }

        public void UpdateGame(Game game)
        {
            gameRepository.UpdateGame(game);
        }

        private static decimal AverageScore(List<Review> reviews)
        {
            decimal score = 0;
            if (reviews.Count == 0)
                return score;
            foreach (var review in reviews)
                score += review.Score;
            return score / reviews.Count;
        }

        private static (TimeInfo normal, TimeInfo full) GetTimeInfo(List<TimeRecord> timeRecords)
        {
            IEnumerable<TimeRecord> timeRecordsNormal = from rec in timeRecords
                                                        where rec.Type == TimeRecordType.NORMAL
                                                        select rec;
            IEnumerable<TimeRecord> timeRecordsFull = from rec in timeRecords
                                                      where rec.Type == TimeRecordType.FULL
                                                      select rec;
            TimeInfo timeInfoN = new TimeInfo(TimeRecordType.NORMAL, AvgTime(timeRecordsNormal), MidTime(timeRecordsNormal), MaxTime(timeRecordsNormal), MinTime(timeRecordsNormal), timeRecordsNormal.Count());
            TimeInfo timeInfoF = new TimeInfo(TimeRecordType.FULL, AvgTime(timeRecordsFull), MidTime(timeRecordsFull), MaxTime(timeRecordsFull), MinTime(timeRecordsFull), timeRecordsFull.Count());
            return (timeInfoN, timeInfoF);
        }

        private static TimeSpan AvgTime(IEnumerable<TimeRecord> records)
        {
            TimeSpan time = new TimeSpan(0);
            if (!records.Any())
                return time;
            foreach (var rec in records)
                time += rec.TimeStamp;
            return time / records.Count();
        }

        private static TimeSpan MidTime(IEnumerable<TimeRecord> records)
        {
            if (!records.Any())
                return new TimeSpan(0);
            IEnumerable<TimeRecord> times = from rec in records
                                            orderby rec.TimeStamp ascending
                                            select rec;
            return times.ToList()[records.Count() / 2].TimeStamp;
        }

        private static TimeSpan MaxTime(IEnumerable<TimeRecord> records)
        {
            return !records.Any() ? new TimeSpan(0) : records.Max(r => r.TimeStamp);
        }

        private static TimeSpan MinTime(IEnumerable<TimeRecord> records)
        {
            return !records.Any() ? new TimeSpan(0) : records.Min(r => r.TimeStamp);
        }

        private static List<Review> SelectMostReviews(List<Review> reviews, int count)
        {
            IEnumerable<Review> mostReviews = from rev in reviews
                                              orderby rev.Score ascending
                                              select rev;
            if (mostReviews.Count() < count * 2)
                return mostReviews.ToList();
            IEnumerable<Review> res = mostReviews.Take(count);
            return res.Concat(mostReviews.TakeLast(count)).ToList();
        }
    }
}
