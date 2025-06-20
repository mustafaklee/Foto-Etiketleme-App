namespace WebAPI.Models.Dtos
{
    public class FolderFotoDto
    {
        public int FolderId { get; set; }
        public string FolderPath { get; set; }
        public List<FotoDto> Fotograflar { get; set; }
    }
}
