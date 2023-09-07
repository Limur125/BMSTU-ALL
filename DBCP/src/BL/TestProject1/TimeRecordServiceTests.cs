namespace TestProject1
{
    [TestClass]
    public class TimeRecordServiceTests
    {
        [TestMethod]
        public void SaveTimeRecordTest()
        {
            TimeRecordRepositoryStub timeRecordRepository = new TimeRecordRepositoryStub(new List<TimeRecord>());
            ITimeRecordService timeRecordService = new TimeRecordService(timeRecordRepository);
            TimeRecord rec = new TimeRecord(1, new TimeSpan(12, 0, 0), TimeRecordType.NORMAL, "User1");

            timeRecordService.AddTimeRecord(rec);

            List<TimeRecord> actList = timeRecordRepository.records;
            Assert.AreEqual(1, actList.Count);
            TimeRecord actual = actList[0];
            Assert.AreEqual(new TimeSpan(12, 0, 0), actual.TimeStamp);
            Assert.AreEqual("User1", actual.UserLogin);
            Assert.AreEqual(TimeRecordType.NORMAL, actual.Type);
        }
        [TestMethod]
        public void DeleteTimeRecordTest()
        {
            TimeRecord rec = new TimeRecord(1, new TimeSpan(12, 0, 0), TimeRecordType.NORMAL, "User1");
            List<TimeRecord> records = new List<TimeRecord>()
            {
                rec,
                new TimeRecord(1, new TimeSpan(20, 0, 0), TimeRecordType.FULL, "User2"),
            };
            TimeRecordRepositoryStub timeRecordRepository = new TimeRecordRepositoryStub(records);
            ITimeRecordService timeRecordService = new TimeRecordService(timeRecordRepository);

            timeRecordService.DeleteTimeRecord(rec);

            List<TimeRecord> actList = timeRecordRepository.records;
            Assert.AreEqual(1, actList.Count);
            TimeRecord actual = actList[0];
            Assert.AreEqual(new TimeSpan(20, 0, 0), actual.TimeStamp);
            Assert.AreEqual("User2", actual.UserLogin);
            Assert.AreEqual(TimeRecordType.FULL, actual.Type);
        }

        [TestMethod]
        public void GetTimeRecordsTest()
        {
            List<TimeRecord> records = new List<TimeRecord>()
            {
                new TimeRecord(1, new TimeSpan(12, 0, 0), TimeRecordType.NORMAL, "User1"),
                new TimeRecord(1, new TimeSpan(20, 0, 0), TimeRecordType.FULL, "User2"),
            };
            TimeRecordRepositoryStub timeRecordRepository = new TimeRecordRepositoryStub(records);
            ITimeRecordService timeRecordService = new TimeRecordService(timeRecordRepository);

            List<TimeRecord> actList = timeRecordService.GetTimeRecords(1);

            Assert.AreEqual(2, actList.Count);
            TimeRecord actual = actList[0];
            Assert.AreEqual(new TimeSpan(12, 0, 0), actual.TimeStamp);
            Assert.AreEqual("User1", actual.UserLogin);
            Assert.AreEqual(TimeRecordType.NORMAL, actual.Type);
            actual = actList[1];
            Assert.AreEqual(new TimeSpan(20, 0, 0), actual.TimeStamp);
            Assert.AreEqual("User2", actual.UserLogin);
            Assert.AreEqual(TimeRecordType.FULL, actual.Type);
        }
    }
}
