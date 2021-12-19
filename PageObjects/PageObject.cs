using Microsoft.Playwright;

namespace PageObjects
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
