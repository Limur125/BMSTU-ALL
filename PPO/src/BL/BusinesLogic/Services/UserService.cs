namespace BusinesLogic
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public void LogIn(User user)
        {
            if (!userRepository.CheckUser(user))
                throw new LoginException("User not found");
        }
        public void Register(User user)
        {
            userRepository.AddUser(user);
        }
    }
}
