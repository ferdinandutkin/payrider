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

            Logger.Information($"Limit price: {limitPrice}");

            var currentSymbol = await _tradingPage.GetCurrentCompanySymbol();

            Logger.Information($"Current company: {currentSymbol}");

            await _tradingPage.EnterAmount(limitPrice);

            Logger.Information($"Entered amount: {limitPrice}");

            await _tradingPage.ClickPaperTrade();

            var activeTrades = await _tradingPage.GetUpdatedActiveTradesAsync();

            Assert.Contains(new Trade() { Price = limitPrice, Symbol = currentSymbol, Side = Side.Buy }, activeTrades);
        }


     

        [Theory]
        [InlineData(1000000)]
        public async Task PlaceOrder_WhenNetAccountValueIsInsufficient_ResultsInInsufficientFundsPopup(int amount)
        {

            await _tradingPage.GoToPaperTrading();

            await _tradingPage.SelectSide(Side.Buy);

            await _tradingPage.SelectUnitOfMeasure(UnitOfMeasure.Quantity);

            await _tradingPage.EnterAmount(amount);

            Logger.Information($"Entered amount: {amount}");

            await _tradingPage.ClickPaperTrade();

            var hasInsufficientFundsPopup = await _tradingPage.HasInsufficientFundsPopup();

            Assert.True(hasInsufficientFundsPopup);

        }


        [Fact]
        public async Task PlaceOffer_WhenShareAreInsufficient_DoesntResultInError()
        {

            await _tradingPage.GoToPaperTrading();

            var currentSymbol = await _tradingPage.GetCurrentCompanySymbol();

            var quantityOfСurrentPosition = await _tradingPage.GetQuantityOfPosition(currentSymbol);

            Logger.Information($"quantity of current position {quantityOfСurrentPosition}");

            await _tradingPage.SelectSide(Side.Sell);

            await _tradingPage.SelectUnitOfMeasure(UnitOfMeasure.Quantity);

            var amount = quantityOfСurrentPosition + 1;

            await _tradingPage.EnterAmount(amount);

            Logger.Information($"Entered amount: {amount}");

            var hasNoInsufficientFundsPopup = !await _tradingPage.HasInsufficientFundsPopup();

            Assert.True(hasNoInsufficientFundsPopup);
        }

    }
}
