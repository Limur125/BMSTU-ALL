namespace BusinesLogic
{
    public record class Game
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public DateTime ReleaseDate { get; init; }
        public string Developer { get; init; }
        public string Publisher { get; init; }
        public string Platform { get; init; }
        public Game(int id, string title, DateTime releaseDate, string developer, string publisher, string platform)
        {
            Id = id;
            Title = title;
            ReleaseDate = releaseDate;
            Developer = developer;
            Publisher = publisher;
            Platform = platform;
        }
    }
}
