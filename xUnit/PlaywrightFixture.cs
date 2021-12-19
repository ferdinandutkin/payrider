using Microsoft.Playwright;
using System;

namespace xUnitTests
{
    public partial class PlaywrightFixture : IDisposable
    {
        private readonly IConfiguration _configuration;

        public IBrowser Browser { get; private set; }

        public IPlaywright Playwright { get; private set; }

        public IBrowserType BrowserType { get; set; }
        public IBrowserContext Context { get; set; }

       
        public PlaywrightFixture()
        {
            _configuration = new EnviromentConfiguration();

            Playwright = Microsoft.Playwright.Playwright.CreateAsync().Result;
            BrowserType = Playwright[_configuration.BrowserName];
            Browser = BrowserType.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = _configuration.Headless,
                SlowMo = _configuration.SlowMo,
            }).Result;

            Context = Browser.NewContextAsync().Result;

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
