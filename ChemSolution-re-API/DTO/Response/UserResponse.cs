using ChemSolution_re_API.Entities.Enums;

namespace ChemSolution_re_API.DTO.Response
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string UserEmail { set; get; } = string.Empty;
        public string UserName { set; get; } = string.Empty;
        public DateTime DateOfBirth { set; get; }
        public int Balance { set; get; }
        public int Rating { set; get; }
        public int Honesty { set; get; }
        public Role Role { set; get; }
    }
}
