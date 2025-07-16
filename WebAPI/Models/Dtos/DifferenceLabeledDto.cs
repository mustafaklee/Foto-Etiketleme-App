namespace WebAPI.Models.Dtos
{
    public class DifferenceLabeledDto
    {
        public int Doctor1Id { get; set; }
        public int Doctor2Id { get; set; }

        public List<LabelCountDto> Doctor1BiradsCounts { get; set; }
        public List<LabelCountDto> Doctor2BiradsCounts { get; set; }

        public List<LabelCountDto> Doctor1FindingCounts { get; set; }
        public List<LabelCountDto> Doctor2FindingCounts { get; set; }

    }
}
