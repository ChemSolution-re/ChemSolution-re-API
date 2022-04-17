namespace ChemSolution_re_API.Entities
{
    public class Status
    {
        public Guid Id { set; get; }
        public string StatusName { set; get; } = string.Empty;

        public List<Request> Requests { set; get; } = new List<Request>();
    }
}
