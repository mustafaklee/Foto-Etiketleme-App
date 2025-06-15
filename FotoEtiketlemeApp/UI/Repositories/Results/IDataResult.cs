namespace UI.Repositories.Results;
    public interface IDataResult<T> : IResult
    {
        T Data { get; }
    }
