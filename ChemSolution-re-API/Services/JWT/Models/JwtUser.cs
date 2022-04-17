using ChemSolution_re_API.Entities;

namespace ChemSolution_re_API.Services.JWT.Models
{
    public class JwtUser
    {
        public Guid Id { set; get; }
        public Role Role { set; get; }
    }
}
