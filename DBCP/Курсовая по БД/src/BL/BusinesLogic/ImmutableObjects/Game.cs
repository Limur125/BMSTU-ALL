namespace BusinesLogic
{
    public class Game
    {
        public int Id { get; }
        public string Title { get; }
        public DateTime ReleaseDate { get; }
        public string Developer { get; }
        public string Publisher { get; }
        public string Genres { get; }
        public Game(int id, string title, DateTime releaseDate, string developer, string publisher, string genre)
        {
            Id = id;
            Title = title;
            ReleaseDate = releaseDate;
            Developer = developer;
            Publisher = publisher;
            Genres = genre;
        }
    }
}
