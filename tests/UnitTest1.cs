using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using System.Threading.Tasks;
using WebullApi;

namespace tests
{

    public class Test1 : ContextTest
    {

        private TradingPage _tradingPage;
       
   
        [SetUp]
        public async Task Setup()
        {
                  
            string email = "ferdinandutkin@gmail.com";

            string password = "Asmallamountofmilk13";

            var page = await Context.NewPageAsync();
            
            var loginPage = await new HomePage(page)
                .ClickLoginAsync();

            var centerPage = await loginPage.LoginViaEmailAsync(email, password);

            _tradingPage = await centerPage.ClickTrade();

        }




        [Test] 

        public async Task Test()
        {
            await _tradingPage.Trade();
        }

   
    }


    


    //private async Task LoginAsync()û
    //{
    //    playwright = await Playwright.CreateAsync();
    //    browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
    //    { Headless = false, SlowMo = 50, });
    //    Context = await browser.NewContextAsync();
    //    await Context.StorageStateAsync(new BrowserContextStorageStateOptions
    //    {
    //        Path = "state.json"
    //    });
    //    page = await Context.NewPageAsync();
    //    await page.GotoAsync(HttpsWwwDemoblazeComCartHtml);
    //    await page.ClickAsync("a:has-text('Log In')");
    //    await Helpers.Login("tyschenk@20@gmail.com", "12345678", page);
    //}
}
