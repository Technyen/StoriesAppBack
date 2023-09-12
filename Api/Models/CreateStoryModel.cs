namespace Api.Models
{
    public class CreateStoryModel
    {
        public string? Title { get; set; }
        public string? Category { get; set; }
        public int? AgeSuggested { get; set; }
        public string? Description { get; set; }
        public IFormFile? FormFile { get; set; }
    }
}
