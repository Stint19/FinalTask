namespace FinalTask.Application.Exceptions
{
    public class LoginFailedException : Exception
    {
        public LoginFailedException() : base("Incorrect login or password")
        {

        }
    }
}
