using ChemSolution_re_API.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace ChemSolution_re_API.DTO.Request
{
    public class UpdateBlogPost
    {
        public Guid BlogPostId { set; get; }
        public string Title { set; get; } = string.Empty;
        [Required]
        [EnumDataType(typeof(BlogPostCategory))]
        public string BlogPostCategory { set; get; } = string.Empty;
        public string Information { set; get; } = string.Empty;
        public string Image { set; get; } = string.Empty;
        public bool IsLocked { set; get; }
    }
}
