namespace BusinesLogic
{
    public class TimeRecord
    {
        public int GameId { get; }
        public TimeSpan TimeStamp { get; }
        public TimeRecordType Type { get; }
        public string UserLogin { get; }
        public TimeRecord(int gameId, TimeSpan time, TimeRecordType type, string user)
        {
            GameId = gameId;
            TimeStamp = time;
            Type = type;
            UserLogin = user;
        }
    }
    public enum TimeRecordType
    {
        NORMAL,
        FULL
    }
}