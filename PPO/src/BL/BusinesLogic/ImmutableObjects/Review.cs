namespace BusinesLogic
{
    public record class Review
    {
        public int GameId { get; init; }
        public string UserLogin { get; init; }
        public uint Score { get; init; }
        public string Text { get; init; }
        public DateTime PublicationDate { get; init; }
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
