namespace WebAPI.Repositories.Results;

public class SuccessResult : Result
{
    // Message propertysi doldurulacak ve Success propertysi true atanacak
    public SuccessResult(string message) : base(true, message)
    {
    }
    // Sadece Success propertsi true atanacak
    public SuccessResult() : base(true)
    {
    }
}
