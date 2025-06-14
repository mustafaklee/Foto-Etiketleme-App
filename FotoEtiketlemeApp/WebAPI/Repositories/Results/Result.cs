namespace WebAPI.Repositories.Results;

public abstract class Result : IResult
{
    //Constructor metot
    public Result(bool success)
    {
        Success = success;
    }
    public Result(bool success, string message) : this(success)
    {
        Message = message;
    }
    public bool Success { get; }
    public string Message { get; }
}
