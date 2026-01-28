namespace Ncct.Blazor.Sample.Shared.Interfaces;

public interface IEmailService
{
    void SendEmail(string to, string subject, string body);
}
