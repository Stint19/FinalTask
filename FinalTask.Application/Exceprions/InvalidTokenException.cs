namespace FinalTask.Application.Exceprions
{
    public class InvalidTokenException : Exception
    {
        public InvalidTokenException() : base("Invalid access token or refresh token") 
        { }
    }
}
