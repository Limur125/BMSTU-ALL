using BusinesLogic;
using SqlServerDatabase.Repositories;
using System.Data.Linq;

namespace SqlServerDatabase
{
    public class SqlServerTimeRecordRepository : SqlServerRepository, ITimeRecordRepository
    {
        public SqlServerTimeRecordRepository() : base() { }
        public void AddTimeRecord(TimeRecord record)
        {
            DataContext db = new DataContext(userConnection);
            Table<TimeRecords> timeRecordTable = db.GetTable<TimeRecords>();
            TimeRecords r = new TimeRecords()
            {
                GameId = record.GameId,
                UserLogin = record.UserLogin,
                Minutes = record.TimeStamp.Minutes,
                Hours = record.TimeStamp.Hours,
                Type = TimeRecordTypeToInt(record.Type)
            };
            timeRecordTable.InsertOnSubmit(r);
            db.SubmitChanges();
        }

        public void DeleteTimeRecord(TimeRecord record)
        {
            DataContext db = new DataContext(adminConnection);
            TimeRecords rec = db.GetTable<TimeRecords>().Where(r => r.GameId == record.GameId && r.UserLogin == record.UserLogin).First();
            db.GetTable<TimeRecords>().DeleteOnSubmit(rec);
            db.SubmitChanges();
        }

        public List<TimeRecord> GetGameTimeRecords(int id)
        {
            DataContext db = new DataContext(guestConnection);
            IQueryable<TimeRecords> gameTimeRecords = from r in db.GetTable<TimeRecords>()
                                                      where r.GameId == id
                                                      select r;
            List<TimeRecord> records = new List<TimeRecord>();
            foreach (var rec in gameTimeRecords)
                records.Add(new TimeRecord(rec.GameId, new TimeSpan(rec.Hours, rec.Minutes, 0), IntToTimeRecordType(rec.Type), rec.UserLogin));
            return records;
        }

        private static int TimeRecordTypeToInt(TimeRecordType trt)
        {
            return trt switch
            {
                TimeRecordType.NORMAL => 0,
                TimeRecordType.FULL => 1,
                _ => throw new TimeRecordTypeException("Invalid time record type")
            };
        }

        private static TimeRecordType IntToTimeRecordType(int t)
        {
            return t switch
            {
                0 => TimeRecordType.NORMAL,
                1 => TimeRecordType.FULL,
                _ => throw new TimeRecordTypeException("Invalid time record type")
            };
        }


    }
}
