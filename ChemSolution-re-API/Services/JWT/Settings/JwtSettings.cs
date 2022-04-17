using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ChemSolution_re_API.Services.JWT.Settings
{
    public class JwtSettings
    {
        public const string ISSUER = "ChemSolutionAPI";
        public const string AUDIENCE = "ChemSolutionClient";
        private static readonly string KEY = RandomKey.CreateKey(30);
        public const int LIFETIME = 60;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
