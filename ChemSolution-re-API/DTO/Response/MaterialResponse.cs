using ChemSolution_re_API.Entities;

namespace ChemSolution_re_API.DTO.Response
{
    public class MaterialResponse
    {
        public Guid Id { get; set; }
        public string Image { get; set; } = string.Empty;
        public string Formula { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Info { get; set; } = string.Empty;
        public MaterialGroup MaterialGroup { set; get; }

        public List<ElementMaterialResponse> ElementMaterials { set; get; } = new List<ElementMaterialResponse>();
    }
}
