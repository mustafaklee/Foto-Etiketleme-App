namespace WebAPI.Repositories
{
    public interface IApiRepository
    {
        Task<string> GetProtectedDataAsync();
    }
}
