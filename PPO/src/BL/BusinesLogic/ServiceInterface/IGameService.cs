namespace BusinesLogic
{
    public interface IGameService
    {
        public void AddGame(Game game);
        public GameInfo GetGame(string title);
        public List<string> GetLibrary();
        public void DeleteGame(string title);
        public void UpdateGame(Game game);
    }
}
