using System.Data.Linq.Mapping;

namespace SqlServerDatabase
{
    [Table]
    public class Users
    {
        [Column(IsPrimaryKey = true)]
        public string Login { get; set; }
        [Column]
        public string Password { get; set; }
    }
}
