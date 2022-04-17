namespace ChemSolution_re_API.Services.Email
{
    public interface IEmailService
    {
        public Task SendEmailAsync(string email, string subject, string message);
    }
}
