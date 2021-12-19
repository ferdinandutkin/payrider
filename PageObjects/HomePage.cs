using Microsoft.Playwright;
using System.Threading.Tasks;

namespace PageObjects
{
    public class HomePage : PageObject
    {
        public HomePage(IPage page) : base(page)
        {
        }

        public async Task<LoginPage> ClickLoginAsync()
        {
            await Page.GotoAsync("https://www.webull.com/");
            await Page.ClickAsync("text=LOG IN");
            return new LoginPage(Page);
        }
    }
}
