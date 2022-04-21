using System.ComponentModel.DataAnnotations;

namespace ChemSolution_re_API.DTO.Request
{
    public class VerifyEmailRequest
    {
        [Required]
        public string Token { get; set; } = string.Empty;
    }
}
