using BusinesLogic;
using SqlServerDatabase.Repositories;
using System.Data.Linq;

namespace SqlServerDatabase
{
    public class SqlServerGameRepository : SqlServerRepository, IGameRepository
    {
        public SqlServerGameRepository() : base() { }
        public void AddGame(Game game)
        {
            DataContext db = new DataContext(adminConnection);
            Table<Games> gameTable = db.GetTable<Games>();
            Games g = new Games()
            {
                Title = game.Title,
                ReleaseDate = game.ReleaseDate,
                Developer = game.Developer,
                Publisher = game.Publisher,
                Platform = game.Platform
            };
            gameTable.InsertOnSubmit(g);
            try
            {
                db.SubmitChanges();
            }
            catch (DuplicateKeyException e)
            {
                throw new GameAlreadyExistsException(e.Message);
            }
        }

        public void DeleteGame(string title)
        {
            DataContext db = new DataContext(adminConnection);
            Games game = db.GetTable<Games>().Single(g => g.Title == title);
            db.GetTable<Games>().DeleteOnSubmit(game);
            IEnumerable<Reviews> revs = from r in db.GetTable<Reviews>()
                                        where r.GameId == game.Id
                                        select r;
            db.GetTable<Reviews>().DeleteAllOnSubmit(revs);
            IEnumerable<TimeRecords> recs = from r in db.GetTable<TimeRecords>()
                                            where r.GameId == game.Id
                                            select r;
            db.GetTable<TimeRecords>().DeleteAllOnSubmit(recs);
            db.SubmitChanges();
        }

        public List<Game> GetAllGames()
        {
            DataContext db = new DataContext(guestConnection);
            Table<Games> gameTable = db.GetTable<Games>();
            List<Game> games = new List<Game>();
            foreach (var game in gameTable)
                games.Add(new Game(game.Id, game.Title, game.ReleaseDate, game.Developer, game.Publisher, game.Platform));
            return games;
        }

        public Game GetGame(string title)
        {
            DataContext db = new DataContext(guestConnection);
            Table<Games> gameTable = db.GetTable<Games>();
            Games game = gameTable.SingleOrDefault(g => g.Title == title);
            return gameTable.Where(g => g.Title == title).Count() == 0
                ? throw new GameNotFoundException("Game not found")
                : new Game(game.Id, game.Title, game.ReleaseDate, game.Developer, game.Publisher, game.Platform);
        }

        public void UpdateGame(Game game)
        {
            DataContext db = new DataContext(adminConnection);
            Table<Games> gameTable = db.GetTable<Games>();
            IQueryable<Games> newGame = gameTable.Where(g => g.Id == game.Id);
            if (newGame == null)
                throw new GameNotFoundException("Game not found");
            newGame.Single().Title = game.Title;
            newGame.Single().ReleaseDate = game.ReleaseDate;
            newGame.Single().Developer = game.Developer;
            newGame.Single().Publisher = game.Publisher;
            newGame.Single().Platform = game.Platform;
            db.SubmitChanges();
        }
    }
}