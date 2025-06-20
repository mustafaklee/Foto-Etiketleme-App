namespace WebAPI.Models.Domain
{
    public class Doktor
    {
        public Guid  Id { get; set; }
        public string Email { get; set; }

        // Doktorun etiketlediği fotoğraflar
        public ICollection<Folder> Folder { get; set; }
    }
}
