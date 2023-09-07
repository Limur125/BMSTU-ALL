namespace TestProject1
{
    internal class UserRepositoryStub : IUserRepository
    {
        public List<User> users;
        public UserRepositoryStub(List<User> users)
        {
            this.users = users;
        }

        public void AddUser(User user)
        {
            if (users.Find(u => u.Login == user.Login) != null)
                throw new Exception($"User with login {user.Login} already exists");
            users.Add(user);
        }

        public bool CheckUser(User user)
        {
            return users.Find(u => u.Login == user.Login && u.Password == user.Password) != null;
        }
    }
}
