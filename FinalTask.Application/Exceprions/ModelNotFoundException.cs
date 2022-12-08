namespace FinalTask.Application.Exceprions
{
    public class ModelNotFoundException : Exception
    {
        public ModelNotFoundException(string ModelName, int Id)
            : base($"{ModelName} with id - {Id} cannot be found!")
        {

        }

        public ModelNotFoundException()
            : base("Entity was not found!")
        {

        }
    }
}
