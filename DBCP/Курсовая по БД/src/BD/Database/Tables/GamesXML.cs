using System.Data.Linq.Mapping;

namespace SqlServerDatabase
{
    [Table]
    public class GamesXML
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }
        [Column]
        public string Title { get; set; }
        [Column]
        public DateTime ReleaseDate { get; set; }
        [Column]
        public string Developer { get; set; }
        [Column]
        public string Publisher { get; set; }
        [Column]
        public string Genres { get; set; }
    }
}

