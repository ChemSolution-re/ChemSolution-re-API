namespace ChemSolution_re_API.DTO
{
    public class BlogPostDTO
    {
        public Guid BlogPostId { set; get; }
        public string Title { set; get; } = string.Empty;
        public string Category { set; get; } = string.Empty;
        public string Information { set; get; } = string.Empty;
        public string Image { set; get; } = string.Empty;
        public bool IsLocked { set; get; }
    }
}
