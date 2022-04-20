using ChemSolution_re_API.Entities.Enums;

namespace ChemSolution_re_API.DTO.Response
{
    public class RequestResponse
    {
        public Guid Id { set; get; }
        public DateTime DateTimeSended { set; get; }
        public string Theme { set; get; } = string.Empty;
        public string Text { set; get; } = string.Empty;
        public Status Status { get; set; }
    }
}
