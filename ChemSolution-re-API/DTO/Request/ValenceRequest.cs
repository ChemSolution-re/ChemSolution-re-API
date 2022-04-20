using ChemSolution_re_API.Entities;
using System.ComponentModel.DataAnnotations;

namespace ChemSolution_re_API.DTO.Request
{
    public class ValenceRequest
    {
        [Required]
        [EnumDataType(typeof(Valence))]
        public string Valence { get; set; } = string.Empty;
    }
}
