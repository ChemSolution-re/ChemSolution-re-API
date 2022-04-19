using ChemSolution_re_API.Entities;

namespace ChemSolution_re_API.DTO.Response
{
    public class AchievementResponse
    {
        public Guid Id { get; set; }
        public string Heading { get; set; } = string.Empty;
        public string ImgAchievement { set; get; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int MoneyReward { get; set; }
        public int RatingReward { get; set; }
        public int CountGoal { get; set; }
        public MaterialGroup MaterialGroup { get; set; }
    }
}
