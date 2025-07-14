using System.Text.Json.Serialization;

namespace WebAPI.Models.Dtos
{
    public class EtiketSecimDto
    {
        [JsonPropertyName("image_id")]
        public int ImageId { get; set; }

        [JsonPropertyName("breast_birads")]
        public int BreastBirads { get; set; }

        [JsonPropertyName("finding_categories")]
        public List<int> FindingCategories { get; set; } = new();
    }

}
