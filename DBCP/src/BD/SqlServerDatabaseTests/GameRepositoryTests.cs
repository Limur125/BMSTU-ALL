using System.Data.Linq;

namespace SqlServerDatabaseTests
{
    [TestClass]
    public class GameRepositoryTests
    {
        private readonly string connection = "Data Source=192.168.233.189,1433;Database=GameTime;User ID=SA;Password=Password_1;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        [TestMethod]
        public void AddGameNormalTest()
        {
            DataContext db = new DataContext(connection);
            int expectedCount = db.GetTable<Games>().Count() + 1;

            SqlServerGameRepository ssgr = new SqlServerGameRepository();
            Game g = new Game(0, "Game1", DateTime.Today, "Developer", "Publisher", "PC");

            ssgr.AddGame(g);

            Table<Games> gameTable = db.GetTable<Games>();
            db.Refresh(RefreshMode.OverwriteCurrentValues, gameTable);
            Games game = gameTable.First(g => g.Title == "Game1");
            Assert.AreEqual(expectedCount, gameTable.Count());
            Assert.IsNotNull(game);
            Assert.AreEqual("Game1", game.Title);
            gameTable.DeleteOnSubmit(game);
            db.SubmitChanges();
        }
        [TestMethod]
        public void DeleteGameNormalTest()
        {
            DataContext db = new DataContext(connection);
            Table<Games> gameTable = db.GetTable<Games>();
            SqlServerGameRepository ssgr = new SqlServerGameRepository();
            Games ing = new Games() { Id = 0, Title = "Game1", ReleaseDate = DateTime.Today, Developer = "Developer", Publisher = "Publisher", Platform = "PC" };
            gameTable.InsertOnSubmit(ing);
            db.SubmitChanges();
            int expectedCount = db.GetTable<Games>().Count() - 1;

            ssgr.DeleteGame("Game1");

            db.Refresh(RefreshMode.OverwriteCurrentValues, gameTable);
            gameTable = db.GetTable<Games>();
            Assert.AreEqual(0, gameTable.Where(g => g.Id == 0).Count());
            Assert.AreEqual(expectedCount, gameTable.Count());
        }
        [TestMethod]
        public void GetGameNormalTest()
        {
            DataContext db = new DataContext(connection);
            Table<Games> gameTable = db.GetTable<Games>();
            int expectedCount = db.GetTable<Games>().Count() + 1;
            SqlServerGameRepository ssgr = new SqlServerGameRepository();
            Games ing = new Games() { Id = 0, Title = "Game1", ReleaseDate = DateTime.Today, Developer = "Developer", Publisher = "Publisher", Platform = "PC" };
            gameTable.InsertOnSubmit(ing);
            db.SubmitChanges();

            Game game = ssgr.GetGame("Game1");

            db.Refresh(RefreshMode.OverwriteCurrentValues, gameTable);
            gameTable = db.GetTable<Games>();
            Assert.AreEqual(ing.Id, game.Id);
            Assert.AreEqual(ing.Title, game.Title);
            Assert.AreEqual(ing.ReleaseDate, game.ReleaseDate);
            Assert.AreEqual(ing.Developer, game.Developer);
            Assert.AreEqual(ing.Publisher, game.Publisher);
            Assert.AreEqual(ing.Platform, game.Platform);
            Assert.AreEqual(expectedCount, gameTable.Count());
            gameTable.DeleteOnSubmit(ing);
            db.SubmitChanges();
        }
        [TestMethod]
        public void GetGameFailTest()
        {
            DataContext db = new DataContext(connection);
            SqlServerGameRepository ssgr = new SqlServerGameRepository();
            Assert.ThrowsException<GameNotFoundException>(() => ssgr.GetGame("Game1"));
        }
        [TestMethod]
        public void GetLibraryNormalTest()
        {
            DataContext db = new DataContext(connection);
            Table<Games> gameTable = db.GetTable<Games>();
            SqlServerGameRepository ssgr = new SqlServerGameRepository();
            Games ing = new Games() { Id = 0, Title = "Game1", ReleaseDate = DateTime.Today, Developer = "Developer", Publisher = "Publisher", Platform = "PC" };
            gameTable.InsertOnSubmit(ing);
            db.SubmitChanges();

            List<Game> games = ssgr.GetAllGames();

            db.Refresh(RefreshMode.OverwriteCurrentValues, gameTable);
            gameTable = db.GetTable<Games>();
            Assert.AreEqual(gameTable.Count(), games.Count);
            gameTable.DeleteOnSubmit(ing);
            db.SubmitChanges();
        }
        [TestMethod]
        public void UpdateGameTest()
        {
            DataContext db = new DataContext(connection);
            Table<Games> gameTable = db.GetTable<Games>();
            SqlServerGameRepository ssgr = new SqlServerGameRepository();
            Games ing = new Games() { Id = 0, Title = "Game1", ReleaseDate = DateTime.Today, Developer = "Developer", Publisher = "Publisher", Platform = "PC" };
            gameTable.InsertOnSubmit(ing);
            db.SubmitChanges();
            db.Refresh(RefreshMode.OverwriteCurrentValues, gameTable);
            Games game = db.GetTable<Games>().Where(g => g.Title == "Game1").Single();
            string updTitle = "Game3", updDeveloper = "DeveloperA", updPublisher = "Publisher", updPlatform = "PC";
            DateTime updReleaseDate = DateTime.Today;
            Game newGame = new Game(game.Id, updTitle, updReleaseDate, updDeveloper, updPublisher, updPlatform);

            ssgr.UpdateGame(newGame);

            db.Refresh(RefreshMode.OverwriteCurrentValues, gameTable);
            Games updGame = db.GetTable<Games>().Where(g => g.Title == newGame.Title).Single();
            Assert.AreEqual(updDeveloper, updGame.Developer);
            Assert.AreEqual(updPublisher, updGame.Publisher);
            Assert.AreEqual(updReleaseDate, updGame.ReleaseDate);
            Assert.AreEqual(updPlatform, updGame.Platform);
            ing.Title = updTitle;
            ing.Developer = updDeveloper;
            ing.Publisher = updPublisher;
            ing.ReleaseDate = updReleaseDate;
            ing.Platform = updPlatform;
            gameTable.DeleteOnSubmit(ing);
            db.SubmitChanges();
        }
    }
}