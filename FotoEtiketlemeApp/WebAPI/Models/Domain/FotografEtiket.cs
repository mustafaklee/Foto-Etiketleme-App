using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Domain
{
    public class FotografEtiket
    {
        [Key]
        public int Id { get; set; }
        public int FotografId { get; set; }
        public Fotograf Fotograf { get; set; }

        public int? EtiketId { get; set; }
        public Etiket? Etiket { get; set; }

        public int DoktorId { get; set; }
        public Doktor Doktor { get; set; }

        public DateOnly? EtiketTarihi { get; set; }
    }
}
