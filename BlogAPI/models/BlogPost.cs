using System;

namespace BlogAPI.models
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime? PostTime { get; set; }
        public int BlogId { get; set; }
    }
}
