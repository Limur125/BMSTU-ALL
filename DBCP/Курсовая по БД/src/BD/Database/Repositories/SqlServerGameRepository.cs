using BusinesLogic;
using SqlServerDatabase.Repositories;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

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
                Platform = game.Genres,
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

        public void AddGameXMLSQL(Game game)
        {
            InsertGameProcedure proc = new InsertGameProcedure(adminConnection);
            proc.InsertGamesXML(game.Title, game.ReleaseDate, game.Developer, game.Publisher, game.Genres);
        }

        public void AddGameXMLApp(Game game)
        {
            DataContext db = new DataContext(adminConnection);
            Table<GamesXML> gameTable = db.GetTable<GamesXML>();
            XmlDocument xml = GetXml(game.Genres);
            GamesXML g = new GamesXML()
            {
                Title = game.Title,
                ReleaseDate = game.ReleaseDate,
                Developer = game.Developer,
                Publisher = game.Publisher,
                Genres = xml.InnerXml,
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
        XmlDocument GetXml(string platform)
        {
            string[] platfSplit = platform.Split(' ');
            XmlDocument xml = new XmlDocument();
            XmlElement root = xml.CreateElement("Genres");
            xml.AppendChild(root);
            foreach (var platf in platfSplit)
            {
                xml["Genres"].AppendChild(xml.CreateElement("genre")).InnerText = platf;
            }
            return xml;
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
            newGame.Single().Platform = game.Genres;
            db.SubmitChanges();
        }
    }

    public class InsertGameProcedure : DataContext
    {
        public InsertGameProcedure(string connectionString) : base(connectionString) { }

        [Function(Name = "InsertGameProcedure")]
        [return: Parameter(DbType = "Int")]
        public int InsertGamesXML(
            [Parameter(Name = "title", DbType = "nvarchar(200)")] string title,
            [Parameter(Name = "releaseDate", DbType = "date")] DateTime releaseDate,
            [Parameter(Name = "developer", DbType = "nvarchar(200)")] string developer,
            [Parameter(Name = "publisher", DbType = "nvarchar(200)")] string publisher,
            [Parameter(Name = "platform", DbType = "nvarchar(200)")] string platform
            )
        {
            IExecuteResult result = ExecuteMethodCall(this, MethodBase.GetCurrentMethod() as MethodInfo, title, releaseDate, developer, publisher, platform);
            return (int) result.ReturnValue;
        }
    }
}