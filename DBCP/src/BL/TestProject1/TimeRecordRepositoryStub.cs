namespace TestProject1
{
    internal class TimeRecordRepositoryStub : ITimeRecordRepository
    {
        public List<TimeRecord> records;
        public TimeRecordRepositoryStub(List<TimeRecord> records)
        {
            this.records = records;
        }
        public void AddTimeRecord(TimeRecord record)
        {
            if (records.Find(r => r.UserLogin == record.UserLogin && r.GameId == record.GameId && r.Type == record.Type) != null)
                throw new Exception($"{record.UserLogin} already left time record for {record.GameId}");
            records.Add(record);
        }

        public void DeleteTimeRecord(TimeRecord record)
        {
            if (records.RemoveAll(r => r.UserLogin == record.UserLogin && r.GameId == record.GameId && r.Type == record.Type) == 0)
                throw new Exception($"Time record not found for {record.UserLogin} {record.GameId}");
        }

        public List<TimeRecord> GetGameTimeRecords(int id)
        {
            return records.FindAll(r => r.GameId == id);
        }
    }
}
