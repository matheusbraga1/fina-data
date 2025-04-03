namespace FinaData.Core.Handlers
{
    public interface IEmailHandler
    {
        Task SendAsync(string to, string subject, string confirmationLink);
    }
}
