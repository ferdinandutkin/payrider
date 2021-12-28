using System;
using Microsoft.Playwright;

namespace xUnitTests.Framework
{
    public class PlaywrightFixture : IPlaywrightFixture
    {
        private readonly IConfiguration _configuration;

        public IBrowser Browser { get; }

        public IPlaywright Playwright { get; }

        public IBrowserType BrowserType { get; }
        public IBrowserContext Context
        {
            get
            {

                var context = Browser.NewContextAsync().Result;
                context.SetDefaultNavigationTimeout(_configuration.DefaultTimeout);
                context.SetDefaultTimeout(_configuration.DefaultTimeout);
                return context;
            }
        }


        public PlaywrightFixture(IConfiguration configuration)
        {
            _configuration = configuration;

            Playwright = Microsoft.Playwright.Playwright.CreateAsync().Result;

            BrowserType = Playwright[_configuration.BrowserName];
            Browser = BrowserType.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = _configuration.Headless,
                SlowMo = _configuration.SlowMo,
            }).Result;

            


        }

        private bool _disposedValue;

        public virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Playwright.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
