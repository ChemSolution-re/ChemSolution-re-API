using ChemSolution_re_API.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace ChemSolution_re_API.DTO.Request
{
    public class UpdateElement
    {
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
        public double MeltingTemperature { get; set; }
        public double BoilingTemperature { get; set; }
        public bool IsLocked { get; set; }
        public string Info { get; set; } = string.Empty;
        public string ImgSymbol { get; set; } = string.Empty;
        public string ImgAtom { get; set; } = string.Empty;
        [Range(0, int.MaxValue)]
        public int Price { get; set; }
        [Range(0, int.MaxValue)]
        public int Group { get; set; }
        [Required]
        [EnumDataType(typeof(ElementCategory))]
        public string ElementCategory { get; set; } = string.Empty;

        public List<ValenceRequest> ElementValences { get; set; } = new List<ValenceRequest>();
    }
}
