using System.Threading.Tasks;
using Core;
using PageObjects;
using Shared;
using Xunit;
using Xunit.Abstractions;
using xUnitTests.Framework;

namespace xUnitTests.Tests
{
    public class CancelActiveTradeTest : WebullTestBase
    {
        private readonly TradingPage _tradingPage;
        public CancelActiveTradeTest(PlaywrightFixture fixture, ICredentialsProvider credentialsProvider, ICaptchaBypassUrlProvider captchaBypassUrlProvider, ITestOutputHelper output) : base(fixture, credentialsProvider, captchaBypassUrlProvider, output)
        {
            _tradingPage = CenterPage.ClickTrade().Result;
            PageUnderTest = _tradingPage;
            Prerequirments();
        }

        public void Prerequirments()
        {
            Task.Run(async () =>
            {
                await _tradingPage.GoToPaperTrading();

                await _tradingPage.SelectSide(Side.Buy);

                await _tradingPage.SelectUnitOfMeasure(UnitOfMeasure.Amount);

                var limitPrice = await _tradingPage.GetLimitPrice();

                await _tradingPage.EnterAmount(limitPrice);

                await _tradingPage.ClickPaperTrade();

                await _tradingPage.GetUpdatedActiveTradesAsync();
            }).Wait();


        }

        [Fact]
        public async Task CancelLastAddedTrade_RemovesItFromActiveTradeList()
        {
            var initial = await _tradingPage.GetActiveTradesAsync();
            await _tradingPage.CancelLastActiveTrade();
            var updated = await _tradingPage.GetUpdatedActiveTradesAsync();

            Assert.NotEqual(updated, initial);
        }



    }
}
