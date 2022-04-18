namespace ChemSolution_re_API.Entities
{
    public class Request
    {
        public Guid Id { set; get; }
        public DateTime DateTimeSended { set; get; } = DateTime.UtcNow;
        public string Theme { set; get; } = string.Empty;
        public string Text { set; get; } = string.Empty;
        public Status Status { get; set; }

        public Guid UserId { set; get; }
        public User User { set; get; } = new();
    }
}
