namespace BusinesLogic
{
    public interface ITimeRecordRepository
    {
        public void AddTimeRecord(TimeRecord record);
        public void DeleteTimeRecord(TimeRecord record);
        public List<TimeRecord> GetGameTimeRecords(int id);
    }
}
