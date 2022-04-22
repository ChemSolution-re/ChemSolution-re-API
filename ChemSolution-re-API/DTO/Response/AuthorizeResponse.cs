using ChemSolution_re_API.Entities.Enums;

namespace ChemSolution_re_API.DTO.Response
{
    public class AuthorizeResponse
    {
        public string AccessToken { set; get; } = string.Empty;
        public Role Role { set; get; }
        public Guid UserId { set; get; }
    }
}
