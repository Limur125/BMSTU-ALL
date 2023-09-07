namespace BusinesLogic
{
    public record class User
    {
        public string Login { get; init; }
        public string Password { get; init; }
        public User(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}
