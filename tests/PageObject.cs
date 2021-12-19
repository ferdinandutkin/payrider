using Microsoft.Playwright;

namespace tests
{
    public abstract class PageObject
    {
        protected readonly IPage Page;

        protected PageObject(IPage page)
        {
            Page = page;
        }

    }
}
