using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Domain
{
    public class Fotograf
    {
        [Key]
        public int Id { get; set; }
        public string FotografPath { get; set; }
        public int FolderId { get; set; }
        public Folder Folder { get; set; }
        public ICollection<FotografEtiket> FotografEtiketleri { get; set; }
    }
}