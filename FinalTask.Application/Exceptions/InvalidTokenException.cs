namespace FinalTask.Application.Exceptions
{
    public class InvalidTokenException : Exception
    {
        public InvalidTokenException() : base("Invalid access token or refresh token") 
        { }
    }
}
