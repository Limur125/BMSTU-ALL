namespace BusinesLogic
{
    public record class GameInfo
    {
        public Game Game { get; init; }
        public decimal AvgScore { get; init; }
        public TimeInfo NormalTime { get; init; }
        public TimeInfo FullTime { get; init; }
        public List<Review> Reviews { get; init; }
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
