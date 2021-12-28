using System.Threading.Tasks;
using PageObjects;
using Shared;
using Xunit;
using Xunit.Abstractions;
using xUnitTests.Framework;

namespace xUnitTests.Tests
{

    public class AddWatchlistItemTest : WebullTestBase 
    {
        private readonly TradingPage _tradingPage;
        public AddWatchlistItemTest(PlaywrightFixture fixture, ICredentialsProvider credentialsProvider, ICaptchaBypassUrlProvider captchaBypassUrlProvider, ITestOutputHelper output) : base(fixture, credentialsProvider, captchaBypassUrlProvider, output)
        {
            _tradingPage = CenterPage.ClickTrade().Result;
            PageUnderTest = _tradingPage;
        }

        public override void Dispose()
        {
            _ = _tradingPage.RemoveLastAddedEntryFromMyWatchlist().Result;

        }


       
        [Fact]
        public async Task AdditionCompanyToWatchlist_AddsIt()
        {
            await _tradingPage.GoToPaperTrading();

            var symbol = await _tradingPage.GetCurrentCompanySymbol();

            await _tradingPage.AddCurrentCompanyToMyWatchList();

            var watchlistItems = await _tradingPage.GetUpdatedWatchListItemsAsync();

            Assert.Contains(new () { Symbol = symbol }, watchlistItems);

        }


         

    }
}
