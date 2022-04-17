using System.ComponentModel.DataAnnotations;

namespace ChemSolution_re_API.DTO.Request
{
    public class RegisterRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string UserName { set; get; } = string.Empty;
        [EmailAddress]
        public string UserEmail { set; get; } = string.Empty;
        public DateTime DateOfBirth { set; get; }
        [Required]
        [StringLength(50, MinimumLength = 8)]
        public string Password { set; get; } = string.Empty;
    }
}
