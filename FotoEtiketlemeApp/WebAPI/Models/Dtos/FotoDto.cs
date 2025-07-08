namespace WebAPI.Models.Dtos
{
    public class FotoDto
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public int laterality_id { get; set; }
        public int view_Position { get; set; }
        public Tags tags { get; set; }
    }

}
