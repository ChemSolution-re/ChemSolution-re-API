namespace ChemSolution_re_API.Entities
{
    public class Request
    {
        public Guid Id { set; get; }
        public DateTime DateTimeSended { set; get; } = DateTime.UtcNow;
        public string Theme { set; get; } = string.Empty;
        public string Text { set; get; } = string.Empty;

        public Guid StatusId { get; set; }
        public Status Status { get; set; } = new();

        public Guid UserId { set; get; }
        public User User { set; get; } = new();
    }
}
