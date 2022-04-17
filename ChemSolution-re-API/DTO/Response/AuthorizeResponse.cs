using ChemSolution_re_API.Entities;

namespace ChemSolution_re_API.DTO.Response
{
    public class AuthorizeResponse
    {
        public string Access_token { set; get; } = string.Empty;
        public Role Role { set; get; }
        public Guid UserId { set; get; }
    }
}
