using Newtonsoft.Json;
using System.Net;
using UI.Models.Dtos;
using UI.Repositories;
using UI.Repositories.Results;
public class ApiRepository : IApiRepository
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    public ApiRepository(IHttpClientFactory httpClientFactory,IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<IDataResult<FotoEtiketDto>> GetProtectedDataAsync(string route)
    {
        var client = _httpClientFactory.CreateClient("AuthorizedClient");
        string apiUrl = $"{_configuration["ApiBaseUrl"]}/api/{route}";
        var response = await client.GetAsync(apiUrl);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return new ErrorDataResult<FotoEtiketDto>(new FotoEtiketDto
            {
                Fotograflar = new List<FotoDto>(),
                Etiketler = new List<EtiketDto>(),  
                hasEtiket = new List<EtiketDto>()
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
            Etiketler = new List<EtiketDto>(),
            hasEtiket = new List<EtiketDto>()
        }
        , $"Hata: {response.StatusCode} - {error}");
    }
}