namespace Shared
{
    public interface ICaptchaBypassUrlProvider
    {
        string GetCaptchaBypassUrlViaEmail(string email, string password);

        string GetCaptchaBypassUrlViaPhoneNumber(string phoneNumber, string password);
    }
}
