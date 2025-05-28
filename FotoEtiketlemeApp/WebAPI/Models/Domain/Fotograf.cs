namespace WebAPI.Models.Domain
{
    public class Fotograf
    {
        public int Id { get; set; }
        public string FotografPath { get; set; }
        public DateOnly EtiketlenmeTarihi { get; set; }
        public int DoktorId { get; set; }

        public ICollection<FotografEtiket> FotografEtiketleri { get; set; }
    }
}
