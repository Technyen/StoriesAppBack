﻿namespace Api.Models
{
    public class EditStoryModel
    {
        public string Id { get; set; } = null!;
        public string? Title { get; set; } 
        public string? Category { get; set; } 
        public int? AgeSuggested { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? FormFile { get; set; }
    }
}
