using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Domain
{
    public class FindingCategoriesEntity
    {
        [Key]
        public int Id { get; set; }

        public int ImageId { get; set; } // FK to Image
        public Image Image { get; set; } 

        public int FindingCategoriesId { get; set; } // FK to FindingCategories
        public FindingCategories FindingCategories { get; set; }

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; } = null!;
    }

}
