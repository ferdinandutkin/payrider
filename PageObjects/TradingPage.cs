using Microsoft.Playwright;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace PageObjects
{
    public class TradingPage : PageObject
    {

        private readonly NumberFormatInfo _currencyFormatter = new() { CurrencyGroupSeparator = ",", CurrencySymbol = "", CurrencyDecimalSeparator = ".", NumberDecimalDigits = 2 };

        public TradingPage(IPage page) : base(page)
        {
        }

        public async Task<TradingPage> ChooseBuy()
        {
            await Page.ClickAsync("text=BuySell >> div");

            return this;
        }


        public async Task<decimal?> GetLimitPrice()
        {
            var f = new Locator()
            var limitPriceInput = await Page.InputValueAsync("text=SideBuySellOrder TypeLIMITAmountLimit PriceTime-in-ForceDayExtended HoursNo >> :nth-match(input[type=\"text\"], 2)");

            if (decimal.TryParse(limitPriceInput, NumberStyles.Any, _currencyFormatter, out var limitPrice))
            {
                return limitPrice;
            }
            return null;
        }
        public async Task<TradingPage> Trade()
        {

            Thread.Sleep(2000);
            await Page.ClickAsync(".webull-Papertrading__");

            await ChooseBuy();
            // Assert.AreEqual("https://app.webull.com/paper", Page.Url);
            // Click text=Order TypeLIMITQuantity >> :nth-match(i, 3)
            await Page.ClickAsync("text=Order TypeLIMITQuantity >> :nth-match(i, 3)");
            // Click text=Order TypeLIMITQuantity >> :nth-match(i, 3)
            await Page.ClickAsync("text=Order TypeLIMITQuantity >> :nth-match(i, 3)");
            // Click text=SideBuySellOrder TypeLIMITAmountLimit PriceTime-in-ForceDayExtended HoursNo >> :nth-match(input[type="text"], 2)
            await Page.ClickAsync("text=SideBuySellOrder TypeLIMITAmountLimit PriceTime-in-ForceDayExtended HoursNo >> :nth-match(input[type=\"text\"], 2)");
            // Press c with modifiers
            var limitPrice = await GetLimitPrice();
            // Click text=Order TypeLIMITAmount >> input[type="text"]
            await Page.ClickAsync("text=Order TypeLIMITAmount >> input[type=\"text\"]");


            

            await Page.FillAsync("text=Order TypeLIMITAmount >> input[type=\"text\"]", limitPrice?.ToString(_currencyFormatter));
            // Fill text=Order TypeLIMITAmount >> input[type="text"] 
            // Click text=Paper Trade
            await Page.ClickAsync("text=Paper Trade");
            // Click text=BuySell >> div


            return this;

        }
    }
}
