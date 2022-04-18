using System.ComponentModel.DataAnnotations;

namespace ChemSolution_re_API.DTO.Request
{
    public class UpdateUser
    {
        public Guid Id { get; set; }
        [StringLength(50, MinimumLength = 3)]
        public string UserName { set; get; } = string.Empty;
        public DateTime DateOfBirth { set; get; }
    }
}
