namespace BusinesLogic
{
    public class Review
    {
        public int GameId { get; }
        public string UserLogin { get; }
        public uint Score { get; }
        public string Text { get; }
        public DateTime PublicationDate { get; }
        public Review(int gameId, string userLogin, uint score, string text, DateTime publicationDate)
        {
            GameId = gameId;
            UserLogin = userLogin;
            Score = score;
            Text = text;
            PublicationDate = publicationDate;
        }
    }
}
