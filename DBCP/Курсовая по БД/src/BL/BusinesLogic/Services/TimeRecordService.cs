namespace BusinesLogic
{
    public class TimeRecordService : ITimeRecordService
    {
        private readonly ITimeRecordRepository timeRecordRepository;
        public TimeRecordService(ITimeRecordRepository timeRecordRepository)
        {
            this.timeRecordRepository = timeRecordRepository;
        }
        public void AddTimeRecord(TimeRecord record)
        {
            timeRecordRepository.AddTimeRecord(record);
        }
        public void DeleteTimeRecord(TimeRecord record)
        {
            timeRecordRepository.DeleteTimeRecord(record);
        }
        public List<TimeRecord> GetTimeRecords(int gameId)
        {
            return timeRecordRepository.GetGameTimeRecords(gameId);
        }
    }
}
