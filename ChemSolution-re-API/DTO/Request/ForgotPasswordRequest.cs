using System.ComponentModel.DataAnnotations;

namespace ChemSolution_re_API.DTO.Request
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
