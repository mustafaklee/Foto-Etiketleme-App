namespace WebAPI.Models.Domain
{
    public class laterality
    {
        public int id { get; set; }
        public string laterality_name { get; set; }
        public ICollection<Image> Image { get; set; }

    }
}
