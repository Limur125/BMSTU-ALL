namespace TestProject1
{
    [TestClass]
    public class UserServiceTests
    {
        [TestMethod]
        public void LogInNormalTest()
        {
            List<User> users = new List<User>() { new User("User1", "123456") };
            IUserRepository userRepository = new UserRepositoryStub(users);
            IUserService userService = new UserService(userRepository);
            User user = new User("User1", "123456");
            try
            {
                userService.LogIn(user);
            }
            catch
            {
                Assert.Fail();
            }
        }
        [TestMethod]
        public void LogInFailTest()
        {
            List<User> users = new List<User>() { new User("User1", "123456") };
            IUserRepository userRepository = new UserRepositoryStub(users);
            IUserService userService = new UserService(userRepository);
            User user = new User("User1", "1232456");
            Assert.ThrowsException<LoginException>(() => userService.LogIn(user));
        }
        [TestMethod]
        public void RegisterTest()
        {
            List<User> users = new List<User>();
            UserRepositoryStub userRepository = new UserRepositoryStub(users);
            IUserService userService = new UserService(userRepository);
            User user = new User("User1", "1232456");

            userService.Register(user);

            List<User> actList = userRepository.users;
            Assert.AreEqual(1, actList.Count);
            User actual = actList[0];
            Assert.AreEqual("User1", actual.Login);
            Assert.AreEqual("1232456", actual.Password);
        }
    }
}
