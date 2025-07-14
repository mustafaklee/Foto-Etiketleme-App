using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Domain
{
    public class BreastBiradsEntity
    {
        [Key]
        public int Id { get; set; }

        public int ImageId { get; set; }
        public Image Image { get; set; }

        public int? BreastBiradsId { get; set; }
        public BreastBirads BreastBirads { get; set; }

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; } = null!;
    }
}
