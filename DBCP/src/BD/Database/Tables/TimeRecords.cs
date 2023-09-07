using System.Data.Linq.Mapping;

namespace SqlServerDatabase
{
    [Table]
    public class TimeRecords
    {
        [Column(IsPrimaryKey = true)]
        public int GameId { get; set; }
        [Column(IsPrimaryKey = true)]
        public string UserLogin { get; set; }
        [Column]
        public int Hours { get; set; }
        [Column]
        public int Minutes { get; set; }
        [Column(IsPrimaryKey =true)]
        public int Type { get; set; }
    }
}
