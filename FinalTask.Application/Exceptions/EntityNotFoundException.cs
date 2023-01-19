namespace FinalTask.Application.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string ModelName, int Id)
            : base($"{ModelName} with id - {Id} cannot be found!")
        {

        }

        public EntityNotFoundException(string username)
            : base($"Entity with name - {username} was not found!")
        {

        }

        public EntityNotFoundException()
            : base($"Entity was not found!") 
        {

        }
    }
}
