namespace FinalTask.Application.Exceptions
{
    public class EntityAlreadyExistException : Exception
    {
        public EntityAlreadyExistException(string name, int id)
            : base($"Entity with name - {name} was already exist with id - {id}")
        {

        }
    }
}
