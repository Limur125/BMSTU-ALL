namespace TestProject1
{
    internal class GameRepositoryStub : IGameRepository
    {
        public List<Game> games;
        public GameRepositoryStub(List<Game> games)
        {
            this.games = games;
        }
        public void AddGame(Game game)
        {
            if (games.Find(g => g.Id == game.Id) is not null)
                throw new Exception("Already exists");
            games.Add(game);
        }

        public void DeleteGame(string title)
        {
            if (games.RemoveAll(x => x.Title == title) == 0)
                throw new Exception("Game not found");
        }

        public List<Game> GetAllGames()
        {
            return games;
        }

        public Game GetGame(string title)
        {
            Game? g = games.Find(g => g.Title == title);
            return g is null ? throw new Exception("Game not found") : g;
        }

        public void UpdateGame(Game game)
        {
            if (games.RemoveAll(g => g.Id == game.Id) == 0)
                throw new Exception("Game not found");
            games.Add(game);
        }
    }
}
