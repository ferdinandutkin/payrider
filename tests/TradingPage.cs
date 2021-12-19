using Microsoft.Playwright;
using System.Threading;
using System.Threading.Tasks;

namespace tests
{
    public class TradingPage : PageObject
    {
        public TradingPage(IPage page) : base(page)
        {
        }

        public async Task<TradingPage> ChooseBuy()
        {
            await Page.ClickAsync("text=BuySell >> div");

            return this;
        }

        public async Task<TradingPage> Trade()
        {

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
            await Page.PressAsync("text=SideBuySellOrder TypeLIMITAmountLimit PriceTime-in-ForceDayExtended HoursNo >> :nth-match(input[type=\"text\"], 2)", "Control+c");
            // Click text=Order TypeLIMITAmount >> input[type="text"]
            await Page.ClickAsync("text=Order TypeLIMITAmount >> input[type=\"text\"]");
            // Fill text=Order TypeLIMITAmount >> input[type="text"]
            await Page.FillAsync("text=Order TypeLIMITAmount >> input[type=\"text\"]", "Control+a");

            await Page.FillAsync("text=Order TypeLIMITAmount >> input[type=\"text\"]", "Control+v");

            Thread.Sleep(400000); ;
            // Click text=Paper Trade
            await Page.ClickAsync("text=Paper Trade");
            // Click text=BuySell >> div
 

            return this;

        }
    }
}
