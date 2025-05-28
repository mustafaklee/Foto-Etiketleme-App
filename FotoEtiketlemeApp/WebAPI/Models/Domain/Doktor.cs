namespace WebAPI.Models.Domain
{
    public class Doktor
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Email { get; set; }

        // Doktorun etiketlediği fotoğraflar
        public ICollection<FotografEtiket> FotografEtiketleri { get; set; }
    }
}
