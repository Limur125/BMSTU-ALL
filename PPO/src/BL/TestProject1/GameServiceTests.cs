namespace TestProject1
{
    [TestClass]
    public class GameServiceTests
    {
        [TestMethod]
        public void GetLibraryNormalTest()
        {
            List<Game> games = new List<Game>() { new Game(1, "Game1", new DateTime(12, 12, 12), "DeveloperA", "PublisherA", "PC") };
            IGameRepository gameRepository = new GameRepositoryStub(games);
            IReviewRepository reviewRepository = new ReviewRepositoryStub(new List<Review>());
            ITimeRecordRepository timeRecordRepository = new TimeRecordRepositoryStub(new List<TimeRecord>());
            IGameService gameService = new GameService(gameRepository, reviewRepository, timeRecordRepository);

            List<string> titles = gameService.GetLibrary();

            Assert.AreEqual(1, titles.Count);
            List<string> expected = new List<string>() { "Game1" };
            for (int i = 0; i < expected.Count; i++)
                Assert.AreEqual(expected[i], titles[i]);
        }
        [TestMethod]
        public void AddGameTest()
        {
            GameRepositoryStub gameRepository = new GameRepositoryStub(new List<Game>());
            IReviewRepository reviewRepository = new ReviewRepositoryStub(new List<Review>());
            ITimeRecordRepository timeRecordRepository = new TimeRecordRepositoryStub(new List<TimeRecord>());
            IGameService gameService = new GameService(gameRepository, reviewRepository, timeRecordRepository);
            Game game2 = new Game(1, "Game2", new DateTime(11, 1, 11), "Developer1", "Publisher2", "PS5");

            gameService.AddGame(game2);

            List<Game> actList = gameRepository.games;
            Assert.AreEqual(1, actList.Count);
            Game actual = actList[0];
            Assert.AreEqual(1, actual.Id);
            Assert.AreEqual("Game2", actual.Title);
            Assert.AreEqual(new DateTime(11, 1, 11), actual.ReleaseDate);
            Assert.AreEqual("Developer1", actual.Developer);
            Assert.AreEqual("Publisher2", actual.Publisher);
            Assert.AreEqual("PS5", actual.Platform);
        }
        [TestMethod]
        public void DeleteGameTest()
        {
            List<Game> games = new List<Game>()
            {
                new Game(1, "Game1", new DateTime(12, 12, 12), "DeveloperA", "PublisherA", "PC"),
                new Game(2, "Game2", new DateTime(11, 1, 11), "Developer1", "Publisher2", "PS5")
            };
            GameRepositoryStub gameRepository = new GameRepositoryStub(games);
            IReviewRepository reviewRepository = new ReviewRepositoryStub(new List<Review>());
            ITimeRecordRepository timeRecordRepository = new TimeRecordRepositoryStub(new List<TimeRecord>());
            IGameService gameService = new GameService(gameRepository, reviewRepository, timeRecordRepository);

            gameService.DeleteGame("Game1");

            List<Game> actList = gameRepository.games;
            Assert.AreEqual(1, actList.Count);
            Game actual = actList[0];
            Assert.AreEqual(2, actual.Id);
            Assert.AreEqual("Game2", actual.Title);
            Assert.AreEqual(new DateTime(11, 1, 11), actual.ReleaseDate);
            Assert.AreEqual("Developer1", actual.Developer);
            Assert.AreEqual("Publisher2", actual.Publisher);
            Assert.AreEqual("PS5", actual.Platform);
        }
        [TestMethod]
        public void UpdateGameTest()
        {
            List<Game> games = new List<Game>()
            {
                new Game(1, "Game1", new DateTime(12, 12, 12), "DeveloperA", "PublisherA", "PC"),
            };
            GameRepositoryStub gameRepository = new GameRepositoryStub(games);
            IReviewRepository reviewRepository = new ReviewRepositoryStub(new List<Review>());
            ITimeRecordRepository timeRecordRepository = new TimeRecordRepositoryStub(new List<TimeRecord>());
            IGameService gameService = new GameService(gameRepository, reviewRepository, timeRecordRepository);
            Game game1 = new Game(1, "Game1", new DateTime(2022, 2, 12), "Developer1", "Publisher1", "PC");

            gameService.UpdateGame(game1);

            List<Game> actList = gameRepository.games;
            Assert.AreEqual(1, actList.Count);
            Game actual = actList[0];
            Assert.AreEqual(1, actual.Id);
            Assert.AreEqual("Game1", actual.Title);
            Assert.AreEqual(new DateTime(2022, 2, 12), actual.ReleaseDate);
            Assert.AreEqual("Developer1", actual.Developer);
            Assert.AreEqual("Publisher1", actual.Publisher);
            Assert.AreEqual("PC", actual.Platform);
        }
        [TestMethod]
        public void GetGameNormalTest()
        {
            List<Game> games = new List<Game>() { new Game(1, "Game1", new DateTime(2021, 12, 12), "DeveloperA", "PublisherA", "PC") };
            IGameRepository gameRepository = new GameRepositoryStub(games);
            List<Review> reviews = new List<Review>()
            {
                new Review(1, "User1", 7, "text", new DateTime(2022, 1, 1)),
                new Review(1, "User2", 7, "text", new DateTime(2022, 7, 15)),
                new Review(1, "User3", 8, "text", new DateTime(2022, 10, 10)),
                new Review(1, "User4", 9, "text", new DateTime(2022, 1, 21)),
                new Review(1, "User5", 5, "text", new DateTime(2022, 4, 1)),
                new Review(1, "User7", 9, "text", new DateTime(2022, 3, 6)),
                new Review(1, "User8", 4, "text", new DateTime(2022, 11, 17))
            };
            IReviewRepository reviewRepository = new ReviewRepositoryStub(reviews);
            List<TimeRecord> records = new List<TimeRecord>()
            {
                new TimeRecord(1, new TimeSpan(19, 0, 0), TimeRecordType.FULL, "User1"),
                new TimeRecord(1, new TimeSpan(13, 0, 0), TimeRecordType.NORMAL, "User2"),
                new TimeRecord(1, new TimeSpan(11, 0, 0), TimeRecordType.NORMAL, "User3"),
                new TimeRecord(1, new TimeSpan(12, 0, 0), TimeRecordType.NORMAL, "User4"),
                new TimeRecord(1, new TimeSpan(16, 0, 0), TimeRecordType.NORMAL, "User1"),
                new TimeRecord(1, new TimeSpan(20, 0, 0), TimeRecordType.FULL, "User2"),
                new TimeRecord(1, new TimeSpan(21, 0, 0), TimeRecordType.FULL, "User5"),
            };
            ITimeRecordRepository timeRecordRepository = new TimeRecordRepositoryStub(records);
            IGameService gameService = new GameService(gameRepository, reviewRepository, timeRecordRepository);

            GameInfo info1 = gameService.GetGame("Game1");

            Assert.AreEqual(1, info1.Game.Id);
            Assert.AreEqual("Game1", info1.Game.Title);
            Assert.AreEqual(new DateTime(2021, 12, 12), info1.Game.ReleaseDate);
            Assert.AreEqual("DeveloperA", info1.Game.Developer);
            Assert.AreEqual("PublisherA", info1.Game.Publisher);
            Assert.AreEqual("PC", info1.Game.Platform);
            Assert.AreEqual(7, info1.AvgScore);
            Assert.AreEqual(4, info1.Reviews.Count);
            Assert.AreEqual(new TimeSpan(13, 0, 0), info1.NormalTime.AvgTime);
            Assert.AreEqual(new TimeSpan(13, 0, 0), info1.NormalTime.MidTime);
            Assert.AreEqual(new TimeSpan(11, 0, 0), info1.NormalTime.MinTime);
            Assert.AreEqual(new TimeSpan(16, 0, 0), info1.NormalTime.MaxTime);
            Assert.AreEqual(new TimeSpan(20, 0, 0), info1.FullTime.AvgTime);
            Assert.AreEqual(new TimeSpan(20, 0, 0), info1.FullTime.MidTime);
            Assert.AreEqual(new TimeSpan(19, 0, 0), info1.FullTime.MinTime);
            Assert.AreEqual(new TimeSpan(21, 0, 0), info1.FullTime.MaxTime);
        }
        [TestMethod]
        public void GetGameZeroReviewsTest()
        {
            List<Game> games = new List<Game>()
            {
                new Game(1, "Game1", new DateTime(2021, 12, 12), "DeveloperA", "PublisherA", "PC")
            };
            IGameRepository gameRepository = new GameRepositoryStub(games);
            List<Review> reviews = new List<Review>();
            IReviewRepository reviewRepository = new ReviewRepositoryStub(reviews);
            List<TimeRecord> records = new List<TimeRecord>()
            {
                new TimeRecord(1, new TimeSpan(19, 0, 0), TimeRecordType.FULL, "User1")
            };
            ITimeRecordRepository timeRecordRepository = new TimeRecordRepositoryStub(records);
            IGameService gameService = new GameService(gameRepository, reviewRepository, timeRecordRepository);

            GameInfo info1 = gameService.GetGame("Game1");

            Assert.AreEqual(1, info1.Game.Id);
            Assert.AreEqual("Game1", info1.Game.Title); ;
            Assert.AreEqual(0, info1.AvgScore);
            Assert.AreEqual(0, info1.Reviews.Count);
        }
        [TestMethod]
        public void GetGameZeroNormalTimeRecordsTest()
        {
            List<Game> games = new List<Game>()
            {
                new Game(1, "Game1", new DateTime(2021, 12, 12), "DeveloperA", "PublisherA", "PC")
            };
            IGameRepository gameRepository = new GameRepositoryStub(games);
            List<Review> reviews = new List<Review>()
            {
                new Review(1, "User1", 7, "text", new DateTime(2022, 1, 1))
            };
            IReviewRepository reviewRepository = new ReviewRepositoryStub(reviews);
            List<TimeRecord> records = new List<TimeRecord>()
            {
                new TimeRecord(1, new TimeSpan(19, 0, 0), TimeRecordType.FULL, "User1"),
                new TimeRecord(1, new TimeSpan(20, 0, 0), TimeRecordType.FULL, "User2"),
                new TimeRecord(1, new TimeSpan(21, 0, 0), TimeRecordType.FULL, "User5"),
            };
            ITimeRecordRepository timeRecordRepository = new TimeRecordRepositoryStub(records);
            IGameService gameService = new GameService(gameRepository, reviewRepository, timeRecordRepository);

            GameInfo info1 = gameService.GetGame("Game1");

            Assert.AreEqual(1, info1.Game.Id);
            Assert.AreEqual("Game1", info1.Game.Title);
            Assert.AreEqual(new TimeSpan(0, 0, 0), info1.NormalTime.AvgTime);
            Assert.AreEqual(new TimeSpan(0, 0, 0), info1.NormalTime.MidTime);
            Assert.AreEqual(new TimeSpan(0, 0, 0), info1.NormalTime.MinTime);
            Assert.AreEqual(new TimeSpan(0, 0, 0), info1.NormalTime.MaxTime);
        }
        [TestMethod]
        public void GetGameLessThan4ReviewsTest()
        {
            List<Game> games = new List<Game>()
            {
                new Game(1, "Game1", new DateTime(2021, 12, 12), "DeveloperA", "PublisherA", "PC")
            };
            IGameRepository gameRepository = new GameRepositoryStub(games);
            List<Review> reviews = new List<Review>()
            {
                new Review(1, "User1", 7, "text", new DateTime(2022, 1, 1)),
                new Review(1, "User2", 7, "text", new DateTime(2022, 7, 15)),
            };
            IReviewRepository reviewRepository = new ReviewRepositoryStub(reviews);
            List<TimeRecord> records = new List<TimeRecord>()
            {
                new TimeRecord(1, new TimeSpan(19, 0, 0), TimeRecordType.FULL, "User1"),
            };
            ITimeRecordRepository timeRecordRepository = new TimeRecordRepositoryStub(records);
            IGameService gameService = new GameService(gameRepository, reviewRepository, timeRecordRepository);

            GameInfo info1 = gameService.GetGame("Game1");

            Assert.AreEqual(1, info1.Game.Id);
            Assert.AreEqual("Game1", info1.Game.Title);
            Assert.AreEqual(2, info1.Reviews.Count);
        }
    }
}