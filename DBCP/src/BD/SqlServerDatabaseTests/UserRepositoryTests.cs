using System.Data.Linq;

namespace SqlServerDatabaseTests
{
    [TestClass]
    public class UserRepositoryTests
    {
        private readonly string connection = "Data Source=192.168.233.189,1433;Database=GameTime;User ID=SA;Password=Password_1;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        [TestMethod]
        public void AddUserNormalTest()
        {
            DataContext db = new DataContext(connection);
            int expectedCount = db.GetTable<Users>().Count() + 1;
            SqlServerUserRepository ssur = new SqlServerUserRepository();
            User usr = new User("User1", "Password");

            ssur.AddUser(usr);

            Table<Users> uTable = db.GetTable<Users>();
            db.Refresh(RefreshMode.OverwriteCurrentValues, uTable);
            Users user = uTable.Single(u => u.Login == "User1");
            Assert.AreEqual(expectedCount, uTable.Count());
            Assert.IsNotNull(user);
            Assert.AreEqual("User1", user.Login);
            uTable.DeleteOnSubmit(user);
            db.SubmitChanges();
        }
        [TestMethod]
        public void CkeckUserTrueTest()
        {
            DataContext db = new DataContext(connection);
            SqlServerUserRepository ssur = new SqlServerUserRepository();
            Users usr = new Users() { Login = "User1", Password = "Password" };
            Table<Users> uTable = db.GetTable<Users>();
            uTable.InsertOnSubmit(usr);
            db.SubmitChanges();

            User cusr = new User("User1", "Password");
            bool res = ssur.CheckUser(cusr);

            Assert.IsTrue(res);
            db.Refresh(RefreshMode.OverwriteCurrentValues, uTable);
            uTable.DeleteOnSubmit(usr);
            db.SubmitChanges();
        }
        [TestMethod]
        public void CkeckUserFalseTest()
        {
            DataContext db = new DataContext(connection);
            SqlServerUserRepository ssur = new SqlServerUserRepository();
            Users usr = new Users() { Login = "User1", Password = "Password" };
            Table<Users> uTable = db.GetTable<Users>();
            uTable.InsertOnSubmit(usr);
            db.SubmitChanges();

            User cusr = new User("User1", "Password1");
            bool res = ssur.CheckUser(cusr);

            Assert.IsFalse(res);
            db.Refresh(RefreshMode.OverwriteCurrentValues, uTable);
            uTable.DeleteOnSubmit(usr);
            db.SubmitChanges();
        }
    }
}
