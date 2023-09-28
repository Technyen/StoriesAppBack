namespace Api.Models
{
    public class CreateStoryModel
    {
        public string Title { get; set; } = null!;
        public string Category { get; set; } = null!;
        public int AgeSuggested { get; set; }
        public string Description { get; set; } = null!;
        public IFormFile FormFile { get; set; } = null!;
    }
}
