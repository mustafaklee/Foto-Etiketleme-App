using UI.Models.Dtos;
using UI.Repositories;

using UI.Repositories.Results;

namespace UI.Repositories
{
    public interface IApiRepository
    {
        Task<IDataResult<FotoEtiketDto>> GetProtectedDataAsync(string route);
    }

}
