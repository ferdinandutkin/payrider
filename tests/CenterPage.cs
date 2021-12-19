using Microsoft.Playwright;
using System.Threading.Tasks;

namespace tests
{
    public class CenterPage : PageObject
    {
        public CenterPage(IPage page) : base(page)
        {

        }

        public async Task<TradingPage> ClickTrade()
        {
            await Page.ClickAsync("text=TRADE");

            var page = await Page.WaitForPopupAsync();

            return new TradingPage(page);
        }

    }
}
