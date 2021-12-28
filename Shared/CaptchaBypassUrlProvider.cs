using WebullApi;

namespace Shared
{
    public class CaptchaBypassUrlProvider : ICaptchaBypassUrlProvider
    {
        private string _captchaBypassUrl;
        public string GetCaptchaBypassUrlViaEmail(string email, string password)
             => _captchaBypassUrl ??= new WebullApiWrapper().GetCaptchaBypassUrlViaEmail(email, password);

        public string GetCaptchaBypassUrlViaPhoneNumber(string phoneNumber, string password)
              => _captchaBypassUrl ??= new WebullApiWrapper().GetCaptchaBypassUrlViaPhoneNumber(phoneNumber, password);
    }
}
