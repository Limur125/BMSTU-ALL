namespace BusinesLogic
{
    public class TimeInfo
    {
        public TimeRecordType Type { get; }
        public TimeSpan AvgTime { get; }
        public TimeSpan MidTime { get; }
        public TimeSpan MaxTime { get; }
        public TimeSpan MinTime { get; }
        public int Count { get; }
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
