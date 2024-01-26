using Medic.Service.Models;

namespace Medic.Service.Interfaces;

public interface IMailSender
{
    public Task<bool> SendAsync(EmailMessage message);
}
