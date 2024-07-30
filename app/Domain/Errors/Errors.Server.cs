namespace Domain.Errors;

public static class Errors
{
    public static class User
    {
        public static readonly Error AlreadyExists = new Error("user.already.exists");
        public static readonly Error IsNotFound = new Error("user.not.found");
        public static readonly Error IsNotBanned = new Error("user.not.banned");
    }
}
