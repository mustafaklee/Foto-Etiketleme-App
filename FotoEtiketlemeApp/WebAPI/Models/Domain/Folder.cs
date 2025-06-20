using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Domain
{
    public class Folder
    {
        [Key]
        public int Id { get; set; }
        public string FolderPath { get; set; }

        public Guid DoktorId { get; set; }
        public Doktor Doktor { get; set; }
        public ICollection<Fotograf> Fotograf { get; set; }

    }
}
