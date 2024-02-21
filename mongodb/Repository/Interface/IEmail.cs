using mongodb.Models;

namespace mongodb.Repository.Interface
{
    public interface IEmail
    {
        Task<int> SendEmailAsync(MailRequest mailrequest);
    }
}
