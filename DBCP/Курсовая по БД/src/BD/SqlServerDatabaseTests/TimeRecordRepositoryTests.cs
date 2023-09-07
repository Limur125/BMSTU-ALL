using System.Data.Linq;

namespace SqlServerDatabaseTests
{
    [TestClass]
    public class TimeRecordRepositoryTests
    {
        private readonly string connection = "Data Source=192.168.233.189,1433;Database=GameTime;User ID=SA;Password=Password_1;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        [TestMethod]
        public void AddTimeRecordNormalTest()
        {
            DataContext db = new DataContext(connection);
            int expectedCount = db.GetTable<TimeRecords>().Count() + 1;
            SqlServerTimeRecordRepository sstrr = new SqlServerTimeRecordRepository();
            TimeRecord rec = new TimeRecord(0, new TimeSpan(12, 0, 0), TimeRecordType.NORMAL, "User1");

            sstrr.AddTimeRecord(rec);

            Table<TimeRecords> trTable = db.GetTable<TimeRecords>();
            db.Refresh(RefreshMode.OverwriteCurrentValues, trTable);
            TimeRecords r = trTable.Single(r => r.GameId == 0 && r.UserLogin == "User1");
            Assert.AreEqual(expectedCount, trTable.Count());
            Assert.IsNotNull(r);
            Assert.AreEqual(0, r.GameId);
            Assert.AreEqual("User1", r.UserLogin);
            trTable.DeleteOnSubmit(r);
            db.SubmitChanges();
        }
        [TestMethod]
        public void GetTimeRecordNormalTest()
        {
            DataContext db = new DataContext(connection);
            SqlServerTimeRecordRepository sstrr = new SqlServerTimeRecordRepository();
            TimeRecords rec = new TimeRecords() { GameId = 0, Hours = 12, Minutes = 0, Type = 0, UserLogin = "User1" };
            Table<TimeRecords> trTable = db.GetTable<TimeRecords>();
            trTable.InsertOnSubmit(rec);
            db.SubmitChanges();

            List<TimeRecord> recs = sstrr.GetGameTimeRecords(0);


            Assert.AreEqual(1, recs.Count);
            foreach (TimeRecord r in recs)
                Assert.AreEqual(0, r.GameId);
            trTable.DeleteOnSubmit(rec);
            db.SubmitChanges();
        }
        [TestMethod]
        public void DeleteGameNormalTest()
        {
            DataContext db = new DataContext(connection);
            Table<TimeRecords> trTable = db.GetTable<TimeRecords>();
            SqlServerTimeRecordRepository ssgr = new SqlServerTimeRecordRepository();
            TimeRecords intr = new TimeRecords() { GameId = 0, Hours = 12, Minutes = 0, Type = 0, UserLogin = "User1" };
            trTable.InsertOnSubmit(intr);
            db.SubmitChanges();
            int expectedCount = db.GetTable<TimeRecords>().Count() - 1;
            TimeRecord rec = new TimeRecord(0, new TimeSpan(12, 0, 0), TimeRecordType.NORMAL, "User1");

            ssgr.DeleteTimeRecord(rec);

            db.Refresh(RefreshMode.OverwriteCurrentValues, trTable);
            trTable = db.GetTable<TimeRecords>();
            Assert.AreEqual(0, trTable.Where(r => r.GameId == 0).Count());
            Assert.AreEqual(expectedCount, trTable.Count());
        }
    }
}
