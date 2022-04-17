namespace ChemSolution_re_API.Entities
{
    public class Category
    {
        public Guid CategoryId { set; get; }
        public string CategoryName { set; get; } = string.Empty;

        public List<Element> Elements { set; get; } = new List<Element>();
    }
}
