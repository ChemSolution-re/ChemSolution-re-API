using System.ComponentModel.DataAnnotations;

namespace ChemSolution_re_API.Entities
{
    public class Material
    {
        public Guid Id { get; set; }
        public string Image { get; set; } = string.Empty;
        [StringLength(50, MinimumLength = 1)]
        public string Formula { get; set; } = string.Empty;
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;
        public string Info { get; set; } = string.Empty;
        public MaterialGroup MaterialGroup { set; get; }


        public List<User> Users = new List<User>();
        public List<ResearchHistory> ResearchHistories { set; get; } = new List<ResearchHistory>();

        public List<Element> Elements { get; set; } = new List<Element>();
        public List<ElementMaterial> ElementMaterials { set; get; } = new List<ElementMaterial>();
    }
}
