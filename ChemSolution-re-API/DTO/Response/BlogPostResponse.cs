using ChemSolution_re_API.Entities;

namespace ChemSolution_re_API.DTO.Response
{
    public class BlogPostResponse
    {
        public Guid BlogPostId { set; get; }
        public string Title { set; get; } = string.Empty;
        public BlogPostCategory BlogPostCategory { set; get; }
        public string Information { set; get; } = string.Empty;
        public string Image { set; get; } = string.Empty;
        public bool IsLocked { set; get; }
    }
}
