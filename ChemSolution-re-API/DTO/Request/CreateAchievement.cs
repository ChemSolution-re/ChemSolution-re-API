using ChemSolution_re_API.Entities;
using System.ComponentModel.DataAnnotations;

namespace ChemSolution_re_API.DTO.Request
{
    public class CreateAchievement
    {
        [StringLength(50, MinimumLength = 5)]
        public string Heading { get; set; } = string.Empty;
        [StringLength(250, MinimumLength = 1)]
        public string ImgAchievement { set; get; } = string.Empty;
        [StringLength(100, MinimumLength = 1)]
        public string Description { get; set; } = string.Empty;
        [Range(0, int.MaxValue)]
        public int MoneyReward { get; set; }
        [Range(0, int.MaxValue)]
        public int RatingReward { get; set; }
        [Range(0, int.MaxValue)]
        public int CountGoal { get; set; }

        [Required]
        [EnumDataType(typeof(MaterialGroup))]
        public string MaterialGroup { get; set; } = string.Empty;
    }
}
