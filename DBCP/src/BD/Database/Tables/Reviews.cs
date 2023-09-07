using System.Data.Linq.Mapping;

namespace SqlServerDatabase
{
    [Table]
    public class Reviews
    {
        [Column(IsPrimaryKey = true)]
        public int GameId { get; set; }
        [Column(IsPrimaryKey = true)]
        public string UserLogin { get; set; }
        [Column]
        public string Text { get; set; }
        [Column]
        public int Score { get; set; }
        [Column]
        public DateTime PublicationDate { get; set; }
    }
}
