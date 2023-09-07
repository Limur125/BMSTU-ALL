namespace BusinesLogic
{
    public record class TimeRecord
    {
        public int GameId { get; init; }
        public TimeSpan TimeStamp { get; init; }
        public TimeRecordType Type { get; init; }
        public string UserLogin { get; init; }
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