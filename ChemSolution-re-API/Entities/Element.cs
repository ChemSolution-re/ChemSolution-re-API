using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChemSolution_re_API.Entities
{
    public class Element
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public int ElementId { get; set; }
        public string Symbol { get; set; } = string.Empty;
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;
        [Range(0, double.MaxValue)]
        public double AtomicWeight { get; set; }
        [Range(0, int.MaxValue)]
        public int NeutronQuantity { get; set; }
        [Range(0, double.MaxValue)]
        public double AtomicRadius { get; set; }
        [Range(0, double.MaxValue)]
        public double Electronegativity { get; set; }
        [Range(0, int.MaxValue)]
        public int EnergyLevels { get; set; }
        [Range(0, int.MaxValue)]
        public int MeltingTemperature { get; set; }
        [Range(0, int.MaxValue)]
        public int BoilingTemperature { get; set; }
        public bool IsLocked { get; set; }
        public string Info { get; set; } = string.Empty;
        public string ImgSymbol { get; set; } = string.Empty;
        public string ImgAtom { get; set; } = string.Empty;
        [Range(0, int.MaxValue)]
        public int Price { get; set; }
        [Range(0, int.MaxValue)]
        public int Group { get; set; }
        public ElementCategory ElementCategory { get; set; }

        public List<ElementValence> ElementValences { get; set; } = new List<ElementValence>();
        public List<User> Users { get; set; } = new List<User>();

        public List<Material> Materials { get; set; } = new List<Material>();
        public List<ElementMaterial> ElementMaterials { set; get; } = new List<ElementMaterial>();
    }
}
