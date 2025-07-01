using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Domain
{
    public class FindingCategories
    {
        [Key]
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public ICollection<FindingCategoriesEntity> FindingCategoriesEntities { get; set; }
    }
}