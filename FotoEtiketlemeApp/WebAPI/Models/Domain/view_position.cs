namespace WebAPI.Models.Domain
{
    public class view_position
    {
        public int id { get; set; }
        public string view_position_name { get; set; }
        public ICollection<Fotograf> Fotograf { get; set; }
    }
}
