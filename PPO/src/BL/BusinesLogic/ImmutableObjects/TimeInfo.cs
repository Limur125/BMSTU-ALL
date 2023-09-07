namespace BusinesLogic
{
    public record class TimeInfo
    {
        public TimeRecordType Type { get; init; }
        public TimeSpan AvgTime { get; init; }
        public TimeSpan MidTime { get; init; }
        public TimeSpan MaxTime { get; init; }
        public TimeSpan MinTime { get; init; }
        public int Count { get; init; }
        public TimeInfo(TimeRecordType type, TimeSpan avgTime, TimeSpan midTime, TimeSpan maxTime, TimeSpan minTime, int count)
        {

            Type = type;
            AvgTime = avgTime;
            MidTime = midTime;
            MaxTime = maxTime;
            MinTime = minTime;
            Count = count;
        }
    }
}
