using Microsoft.Playwright;
using PageObjects;
using System.Threading.Tasks;
using Xunit;

namespace xUnitTests
{


    public class TestOne : IClassFixture<PlaywrightFixture>
    {
        private readonly IBrowserContext _context;
        private TradingPage _tradingPage;

        public TestOne(PlaywrightFixture fixture)
        {
            string email = "ferdinandutkin@gmail.com";

            string password = "Asmallamountofmilk13";

            _context = fixture.Context;
            
            var page = _context.NewPageAsync().Result;

            var loginPage = new HomePage(page)
                .ClickLoginAsync().Result;

            var centerPage = loginPage.LoginViaEmailAsync(email, password).Result;

            _tradingPage = centerPage.ClickTrade().Result;
        }


        [Fact]
        public async Task Test()
        {
            await _tradingPage.Trade();
        }

    }
}
