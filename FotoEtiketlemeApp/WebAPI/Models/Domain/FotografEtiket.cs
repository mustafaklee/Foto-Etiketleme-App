using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Domain
{
    public class FotografEtiket
    {
        [Key]
        public int Id { get; set; }

        public int FotografId { get; set; }
        public Fotograf Fotograf { get; set; }

        public int? BreastBiradsId { get; set; }
        public BreastBirads BreastBirads { get; set; }
    }
}
