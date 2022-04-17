using ChemSolution_re_API.Services.JWT.Models;

namespace ChemSolution_re_API.Services.JWT
{
    public interface IJwtService
    {
        public string GetToken(JwtUser user);
    }
}
