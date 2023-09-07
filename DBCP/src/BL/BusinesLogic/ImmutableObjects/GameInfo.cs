namespace BusinesLogic
{
    public class GameInfo
    {
        public Game Game { get; }
        public decimal AvgScore { get; }
        public TimeInfo NormalTime { get; }
        public TimeInfo FullTime { get; }
        public List<Review> Reviews { get; }
        public GameInfo(Game game, decimal avgScore, TimeInfo normalTime, TimeInfo fullTime, List<Review> reviews)
        {
            Game = game;
            AvgScore = avgScore;
            NormalTime = normalTime;
            FullTime = fullTime;
            Reviews = reviews;
        }
    }
}
