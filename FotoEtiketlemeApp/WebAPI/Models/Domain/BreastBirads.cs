using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Domain
{
    public class BreastBirads
    {
        [Key]
        public int Id { get; set; }
        public string CategoryName { get; set; }

        public ICollection<BreastBiradsEntity> BreastBiradsEntities { get; set; }
    }
}
