namespace WebAPI.Models.Domain
{
    public class FotografEtiket
    {
        public int Id;
        public int FotografId { get; set; }
        public Fotograf Fotograf { get; set; }

        public int EtiketId { get; set; }
        public Etiket Etiket { get; set; }

        public int DoktorId { get; set; }
        public Doktor Doktor { get; set; }
    }
}
