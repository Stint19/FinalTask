namespace FinalTask.Application.Exceprions
{
    public class LoginFailedException : Exception
    {
        public LoginFailedException() : base("Incorrect login or password")
        {

        }
    }
}
