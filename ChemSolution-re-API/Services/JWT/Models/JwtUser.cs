namespace ChemSolution_re_API.Services.JWT.Models
{
    public class JwtUser
    {
        public Guid Id { set; get; }
        public string Role { set; get; } = string.Empty;
    }
}
