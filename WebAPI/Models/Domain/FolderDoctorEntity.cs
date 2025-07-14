namespace WebAPI.Models.Domain
{
    public class FolderDoctorEntity
    {
        public int Id { get; set; }
        public int FolderId { get; set; }
        public Folder Folder { get; set; } = null!;
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; } = null!;
    }
}
