namespace BusinesLogic
{
    public interface ITimeRecordService
    {
        public void AddTimeRecord(TimeRecord record);
        public void DeleteTimeRecord(TimeRecord record);
        public List<TimeRecord> GetTimeRecords(int gameId);
    }
}
