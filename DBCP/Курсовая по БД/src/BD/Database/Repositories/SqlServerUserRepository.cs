using BusinesLogic;
using SqlServerDatabase.Repositories;
using System.Data.Linq;

namespace SqlServerDatabase
{
    public class SqlServerUserRepository : SqlServerRepository, IUserRepository
    {
        public SqlServerUserRepository() : base() { }
        public void AddUser(User user)
        {
            DataContext db = new DataContext(guestConnection);
            Table<Users> userTable = db.GetTable<Users>();
            Users u = new Users()
            {
                Login = user.Login,
                Password = user.Password
            };
            if (userTable.Where(u => u.Login == user.Login).Any())
                throw new UserAlreadyExistsException("User already exists");
            userTable.InsertOnSubmit(u);
            db.SubmitChanges();
        }

        public bool CheckUser(User user)
        {
            DataContext db = new DataContext(guestConnection);
            Table<Users> userTable = db.GetTable<Users>();
            return userTable.Where(u => u.Login == user.Login && u.Password == user.Password).Any();
        }
    }
}
