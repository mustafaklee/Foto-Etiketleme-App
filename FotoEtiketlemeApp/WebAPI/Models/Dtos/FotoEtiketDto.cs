namespace WebAPI.Models.Dtos
{
    public class FotoEtiketDto
    {
        public List<FotoDto> Fotograflar { get; set; }
        public List<EtiketDto> Etiketler { get; set; }
        public List<EtiketDto>? hasEtiket { get; set; }
    }
}
