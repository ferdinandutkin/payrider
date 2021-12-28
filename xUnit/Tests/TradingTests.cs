using System.Threading.Tasks;
using Core;
using PageObjects;
using Shared;
using Xunit;
using Xunit.Abstractions;
using xUnitTests.Framework;

namespace xUnitTests.Tests
{
    public class TradingTests : WebullTestBase
    {
        private readonly TradingPage _tradingPage;
        public TradingTests(PlaywrightFixture fixture, ICredentialsProvider credentialsProvider, ICaptchaBypassUrlProvider captchaBypassUrlProvider, ITestOutputHelper output) : base(fixture, credentialsProvider, captchaBypassUrlProvider, output)
        {
            _tradingPage = CenterPage.ClickTrade().Result;
            PageUnderTest = _tradingPage;
        }

        [Fact]
        public async Task PlaceOrder_WhenNetAccountValueIsSufficient()
        {

            await _tradingPage.GoToPaperTrading();

            await _tradingPage.SelectSide(Side.Buy);

            await _tradingPage.SelectUnitOfMeasure(UnitOfMeasure.Amount);

            var limitPrice = await _tradingPage.GetLimitPrice();

            var currentSymbol = await _tradingPage.GetCurrentCompanySymbol();

            await _tradingPage.EnterAmount(limitPrice);

            await _tradingPage.ClickPaperTrade();

            var activeTrades = await _tradingPage.GetUpdatedActiveTradesAsync();

            Assert.Contains(new Trade() { Price = limitPrice, Symbol = currentSymbol, Side = Side.Buy }, activeTrades);
        }


     

        [Fact]
        public async Task PlaceOrder_WhenNetAccountValueIsInsufficient_ResultsInInsufficientFundsPopup()
        {

            await _tradingPage.GoToPaperTrading();

            await _tradingPage.SelectSide(Side.Buy);

            await _tradingPage.SelectUnitOfMeasure(UnitOfMeasure.Quantity);

            await _tradingPage.EnterAmount(1000000);

            await _tradingPage.ClickPaperTrade();

            var hasInsufficientFundsPopup = await _tradingPage.HasInsufficientFundsPopup();

            Assert.True(hasInsufficientFundsPopup);

        }


        [Fact]
        public async Task PlaceOffer_WhenShareAreInsufficient()
        {

            await _tradingPage.GoToPaperTrading();

            var currentSymbol = await _tradingPage.GetCurrentCompanySymbol();

            var quantityOfСurrentPosition = await _tradingPage.GetQuantityOfPosition(currentSymbol);

            await _tradingPage.SelectSide(Side.Sell);

            await _tradingPage.SelectUnitOfMeasure(UnitOfMeasure.Quantity);

            await _tradingPage.EnterAmount(quantityOfСurrentPosition + 1);

            var hasNoInsufficientFundsPopup = !await _tradingPage.HasInsufficientFundsPopup();

            Assert.True(hasNoInsufficientFundsPopup);
        }

    }
}
