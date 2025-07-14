using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Domain
{
    public class Image
    {
        [Key]
        public int Id { get; set; }
        public string FotografPath { get; set; }
        public int FolderId { get; set; }
        public Folder Folder { get; set; }

        public int laterality_id { get; set; }
        public laterality laterality { get; set; }

        public int view_position_id { get; set; }
        public view_position view_Position { get; set; }
        public ICollection<BreastBiradsEntity> BreastBiradsEntities { get; set; }
        public ICollection<FindingCategoriesEntity> FindingCategoriesEntities { get; set; }

    }
}