namespace BusinesLogic
{
    public interface IUserRepository
    {
        public bool CheckUser(User user);
        public void AddUser(User user);
    }
}
