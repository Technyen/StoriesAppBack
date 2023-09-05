namespace Api.Models
{
    public class EditStoryModel
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public string? Category { get; set; }
        public int AgeSuggested { get; set; }
        public string? Description { get; set; }
    }
}
