namespace EvoltingStore.EmailService
{
    public interface IEmailSender
    {
        Task SendEmailAsyns(string email, string subject, string message);
    }
}
