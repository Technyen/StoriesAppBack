

namespace Api.Entities
{
    public class Story
    {
        public string Type = nameof(Story);
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Category { get; set; } = null!;
        public int AgeSuggested { get; set; }
        public string Description { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;    
    }
}
