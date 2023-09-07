namespace BusinesLogic
{
    public class GameNotFoundException : Exception
    {
        public GameNotFoundException(string? message) : base(message) { }
    }
    public class LoginException : Exception
    {
        public LoginException(string? message) : base(message) { }
    }

    public class TimeRecordTypeException : Exception
    {
        public TimeRecordTypeException(string? message) : base(message) { }
    }

    public class ConfigException : Exception
    {
        public ConfigException(string? message) : base(message) { }
    }

    public class GameAlreadyExistsException : Exception
    {
        public GameAlreadyExistsException(string? message) : base(message) { }
    }
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException(string? message) : base(message) { }
    }
}
