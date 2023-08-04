

namespace Api.Entities
{
    public class Story 
    {
        public string Type = nameof(Story);
        public string Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public int AgeAppropiate { get; set; }
        public string Description { get; set; }
    }
}
