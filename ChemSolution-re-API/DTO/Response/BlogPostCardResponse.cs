using ChemSolution_re_API.Entities.Enums;

namespace ChemSolution_re_API.DTO.Response
{
    public class BlogPostCardResponse
    {
        public Guid BlogPostId { set; get; }
        public string Title { set; get; } = string.Empty;
        public BlogPostCategory BlogPostCategory { set; get; }
        public string Image { set; get; } = string.Empty;
        public bool IsLocked { set; get; }
    }
}
