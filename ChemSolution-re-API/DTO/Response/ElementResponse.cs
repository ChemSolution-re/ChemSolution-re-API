using ChemSolution_re_API.Entities.Enums;

namespace ChemSolution_re_API.DTO.Response
{
    public class ElementResponse
    {
        public int ElementId { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public double AtomicWeight { get; set; }
        public int NeutronQuantity { get; set; }
        public double AtomicRadius { get; set; }
        public double? Electronegativity { get; set; }
        public int EnergyLevels { get; set; }
        public double? MeltingTemperature { get; set; }
        public double? BoilingTemperature { get; set; }
        public bool IsLocked { get; set; }
        public string Info { get; set; } = string.Empty;
        public string ImgSymbol { get; set; } = string.Empty;
        public string ImgAtom { get; set; } = string.Empty;
        public int Price { get; set; }
        public int Group { get; set; }
        public ElementCategory ElementCategory { get; set; }

        public List<MaterialResponse> Materials { get; set; } = new List<MaterialResponse>();
        public List<ElementValenceResponse> ElementValences { get; set; } = new List<ElementValenceResponse>();
    }
}
