using Microsoft.Playwright;
using System.Threading.Tasks;

namespace PageObjects
{
    public class CenterPage : PageObject
    {
        public CenterPage(IPage page) : base(page)
        {

        }

        public async Task<TradingPage> ClickTrade()
        {
            var page = await Page.RunAndWaitForPopupAsync(async () =>
            {
                await Page.ClickAsync("text=TRADE");
            });

            return new TradingPage(page);
        }

    }
}
