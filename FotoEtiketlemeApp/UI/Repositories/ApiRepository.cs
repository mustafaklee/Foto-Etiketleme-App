using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UI.Models.Dtos;
using UI.Repositories;
using UI.Repositories.Results;
using WebAPI.Repositories.Results;

public class ApiRepository : IApiRepository
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ApiRepository(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IDataResult<FotoEtiketDto>> GetProtectedDataAsync(string route)
    {
        var client = _httpClientFactory.CreateClient("AuthorizedClient");
        var response = await client.GetAsync($"https://localhost:7252/api/{route}");
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return new ErrorDataResult<FotoEtiketDto>(new FotoEtiketDto
            {
                Fotograflar = new List<FotoDto>(),
                Etiketler = new List<EtiketDto>()
            }
            , "Yetkisiz erişim. Lütfen giriş yapın.");
        }
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<OperationResultDto<FotoEtiketDto>>(json);
            return new SuccessDataResult<FotoEtiketDto>(result.Data, result.Message);
        }
        // Diğer tüm durumlar
        var error = await response.Content.ReadAsStringAsync();
        return new ErrorDataResult<FotoEtiketDto>(new FotoEtiketDto
        {
            Fotograflar = new List<FotoDto>(),
            Etiketler = new List<EtiketDto>()
        }
        , $"Hata: {response.StatusCode} - {error}");
    }
}