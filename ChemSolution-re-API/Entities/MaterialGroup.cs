namespace ChemSolution_re_API.Entities
{
    public class MaterialGroup
    {
        public Guid Id { set; get; }
        public string GroupName { set; get; } = string.Empty;

        public List<Achievement> Achievements { set; get; } = new List<Achievement>();
        public List<Material> Materials { set; get; } = new List<Material>();
    }
}
