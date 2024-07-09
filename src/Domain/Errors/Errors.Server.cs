namespace Domain.Errors;

public static partial class Errors
{
    public static class User
    {
        public static readonly Error AlreadyExists = new Error("user.already.exists");
        // public static Error AlreadyExists()
        // {
        //     return ;
        // }
    }
}
