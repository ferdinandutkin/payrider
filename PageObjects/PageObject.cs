using Microsoft.Playwright;

namespace PageObjects
{
    public abstract class PageObject
    {
        public readonly IPage Page;

        protected PageObject(IPage page)
        {
            Page = page;
        }

    }
}
