using Microsoft.Playwright;
using Shared;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebullApi;

namespace PageObjects
{
    public class LoginPage : PageObject
    {
        private string _login;
        private string _password;

        public LoginPage(IPage page) : base(page)
        {
        }

        public async Task<LoginPage> SwitchToLoginViaEmailAsync()
        {
            await Page.ClickAsync("text=Email Login");

            return this;
        }

        public async Task<LoginPage> EnterEmailAsync(string email)
        {
            _login = email;

            await Page.FillAsync("[placeholder='Email Address']", email);

            return this;
        }


        public async Task<LoginPage> EnterPasswordAsync(string password)
        {
            _password = password;

            await Page.FillAsync("[placeholder='Password']", password);

            return this;
        }

        public async Task<LoginPage> SubmitAsync()
        {
            await Page.ClickAsync("button:has-text('Log In')");

            return this;
        }


        public async Task<CenterPage> BypassCaptchaAsync(ICaptchaBypassUrlProvider captchaBypassUrlProvider)
        {
            var url = captchaBypassUrlProvider.GetCaptchaBypassUrlViaEmail(_login, _password);

            await Page.GotoAsync(url);

            return new CenterPage(Page);
        }
        
        public async Task<CenterPage> LoginViaEmailAsync(string email, string password, ICaptchaBypassUrlProvider captchaBypassUrlProvider)
        {

            await SwitchToLoginViaEmailAsync();

            await EnterEmailAsync(email);

            await EnterPasswordAsync(password);

            await SubmitAsync();

            return await BypassCaptchaAsync(captchaBypassUrlProvider);

        }
    }
}
