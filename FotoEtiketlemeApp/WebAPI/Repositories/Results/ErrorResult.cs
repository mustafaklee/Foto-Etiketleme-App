namespace WebAPI.Repositories.Results
{
    public class ErrorResult : Result
    {
        // Message propertysi doldurulacak ve Success propertysi false atanacak
        public ErrorResult(string message) : base(false, message)
        {
        }
        // Sadece Success propertsi false atanacak
        public ErrorResult() : base(false)
        {
        }
    }
}
