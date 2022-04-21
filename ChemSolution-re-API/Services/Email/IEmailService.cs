namespace ChemSolution_re_API.Services.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string html, string? from = null);
    }
}
