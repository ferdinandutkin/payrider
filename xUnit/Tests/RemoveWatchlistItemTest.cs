using System.Threading.Tasks;
using PageObjects;
using Shared;
using Xunit;
using Xunit.Abstractions;
using xUnitTests.Framework;

namespace xUnitTests.Tests
{
    public class RemoveWatchlistItemTest : WebullTestBase
    {
        private readonly TradingPage _tradingPage;

        public async Task Prereqirments()
        {
            await _tradingPage.GoToPaperTrading();
            await _tradingPage.AddCurrentCompanyToMyWatchList();
        }
        public RemoveWatchlistItemTest(PlaywrightFixture fixture, ICredentialsProvider credentialsProvider, ICaptchaBypassUrlProvider captchaBypassUrlProvider, ITestOutputHelper output) : base(fixture, credentialsProvider, captchaBypassUrlProvider, output)
        {
            _tradingPage = CenterPage.ClickTrade().Result;
            PageUnderTest = _tradingPage;
            Prereqirments().Wait();
        }

        [Fact]
        public async Task RemovingWatchlistItem_RemovesIt()
        {
           var items =  await _tradingPage.GetUpdatedWatchListItemsAsync();
           await _tradingPage.RemoveLastAddedEntryFromMyWatchlist();
           var updatedItems = await _tradingPage.GetUpdatedWatchListItemsAsync();
           Assert.NotEqual(updatedItems, items);
        }

    }
}
