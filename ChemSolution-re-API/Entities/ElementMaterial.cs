using System.ComponentModel.DataAnnotations;

namespace ChemSolution_re_API.Entities
{
    public class ElementMaterial
    {
        public Guid MaterialId { get; set; }
        public Material? Material { get; set; }
        public int ElementId { get; set; }
        public Element? Element { get; set; }
        [Range(0, int.MaxValue)]
        public int Amount { get; set; }
    }
}
