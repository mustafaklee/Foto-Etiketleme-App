using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Domain
{
    public class Folder
    {
        [Key]
        public int Id { get; set; }
        public string FolderPath { get; set; }
        public int patient_age { get; set; }
        public ICollection<Image> Image { get; set; }

        public ICollection<FolderDoctorEntity> FolderDoctorEntities { get; set; } = new List<FolderDoctorEntity>();
    }
}
