namespace ChemSolution_re_API.DTO.Response
{
    public class BlogPostPageResponse : BlogPostCardResponse
    {
        public string Information { set; get; } = string.Empty;
        public bool IsLiked { set; get; }
    }
}
