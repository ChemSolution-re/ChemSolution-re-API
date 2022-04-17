using System.ComponentModel.DataAnnotations;

namespace ChemSolution_re_API.Entities
{
    public class Achievement
    {
        public int Id { get; set; }
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

        public Guid MaterialGroupId { get; set; }
        public MaterialGroup MaterialGroup { get; set; } = new();
        public List<User> Users { set; get; } = new List<User>();
    }
}
