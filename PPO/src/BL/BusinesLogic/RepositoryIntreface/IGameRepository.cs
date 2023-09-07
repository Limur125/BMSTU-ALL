namespace BusinesLogic
{
    public interface IGameRepository
    {
        public Game GetGame(string title);
        public void AddGame(Game game);
        public void UpdateGame(Game game);
        public void DeleteGame(string title);
        public List<Game> GetAllGames();
    }
}
