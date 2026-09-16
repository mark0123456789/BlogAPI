namespace BlogAPI.models.DTIOs
{
    public class AddBloggerPostDTO
    {
        public string? title { get; set; }
        public string? content { get; set; }
        public int BlogId { get; set; }
    }
}
