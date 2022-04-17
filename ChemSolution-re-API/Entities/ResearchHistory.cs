namespace ChemSolution_re_API.Entities
{
    public class ResearchHistory
    {
        public Guid UserId { set; get; }
        public User User { set; get; } = new();

        public Guid MaterialId { set; get; }
        public Material Material { set; get; } = new();
        public DateTime DateTime { set; get; }
    }
}
