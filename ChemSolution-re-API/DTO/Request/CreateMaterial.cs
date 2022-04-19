using ChemSolution_re_API.Entities;
using System.ComponentModel.DataAnnotations;

namespace ChemSolution_re_API.DTO.Request
{
    public class CreateMaterial
    {
        public string Image { get; set; } = string.Empty;
        [StringLength(50, MinimumLength = 1)]
        public string Formula { get; set; } = string.Empty;
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;
        public string Info { get; set; } = string.Empty;

        [Required]
        [EnumDataType(typeof(MaterialGroup))]
        public string MaterialGroup { set; get; } = string.Empty;
    }
}
