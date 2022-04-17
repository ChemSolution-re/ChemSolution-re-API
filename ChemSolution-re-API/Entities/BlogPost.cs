namespace ChemSolution_re_API.Entities
{
    public class BlogPost
    {
        public Guid BlogPostId { set; get; }
        public string Title { set; get; } = string.Empty;
        public string Category { set; get; } = string.Empty;
        public string Information { set; get; } = string.Empty;
        public string Image { set; get; } = string.Empty;
        public bool IsLocked { set; get; }

        public List<User> Users { set; get; } = new List<User>();
    }
}
