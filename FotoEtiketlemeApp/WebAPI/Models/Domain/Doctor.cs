namespace WebAPI.Models.Domain
{
    public class Doctor
    {
        public Guid  Id { get; set; }
        public string Email { get; set; }
        public ICollection<FolderDoctorEntity> FolderDoctorEntities { get; set; } = null!;

        public ICollection<FindingCategoriesEntity> findingCategoriesEntities { get; set; } = null!;

        public ICollection<BreastBiradsEntity> breastBiradsEntities { get; set; } = null!;

    }
}
