using Microsoft.AspNetCore.Identity;

namespace WebAPI.Models.Domain
{
    public class Doctor : IdentityUser<int>
    {
        public ICollection<FolderDoctorEntity> FolderDoctorEntities { get; set; } = new List<FolderDoctorEntity>();
        public ICollection<FindingCategoriesEntity> FindingCategoriesEntities { get; set; } = new List<FindingCategoriesEntity>();
        public ICollection<BreastBiradsEntity> BreastBiradsEntities { get; set; } = new List<BreastBiradsEntity>();
    }
}
