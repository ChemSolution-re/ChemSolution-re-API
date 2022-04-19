using ChemSolution_re_API.Entities;

namespace ChemSolution_re_API.Response.DTO
{
    public class MaterialResponse
    {
        public Guid Id { get; set; }
        public string Image { get; set; } = string.Empty;
        public string Formula { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Info { get; set; } = string.Empty;
        public MaterialGroup MaterialGroup { set; get; }
    }
}
