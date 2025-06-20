using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Domain
{
    public class FotografEtiket
    {
        [Key]
        public int Id { get; set; }
        public int imageId { get; set; }
        public Fotograf Fotograf { get; set; }
        public int? breast_biradsId { get; set; }
        public BreastBirads? BreastBirads { get; set; }
        public int? finding_categoriesId { get; set; }
        public FindingCategories? FindingCategories { get; set; }

    }
}
