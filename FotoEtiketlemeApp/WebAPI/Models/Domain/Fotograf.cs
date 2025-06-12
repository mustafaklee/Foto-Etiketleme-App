namespace WebAPI.Models.Domain
{
    public class Fotograf
    {
        public int Id { get; set; }
        public string FotografPath { get; set; }

        public ICollection<FotografEtiket> FotografEtiketleri { get; set; }
    }
}
