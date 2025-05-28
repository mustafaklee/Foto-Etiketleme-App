namespace WebAPI.Models.Domain
{
    public class Etiket
    {
        public int Id { get; set; }
        public string EtiketAd { get; set; }

        public ICollection<FotografEtiket> FotografEtiketleri { get; set; }
    }
}
