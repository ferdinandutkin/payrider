using Microsoft.Playwright;
using System;

namespace pw
{
    public partial class BaseTestContextClass : IDisposable
    {
        private readonly IConfiguration _configuration;

        protected IBrowser Browser { get; private set; }

        protected IPlaywright Playwright { get; private set; }

        protected IBrowserType BrowserType { get; set; }
        protected IBrowserContext Context { get; set; }


        public BaseTestContextClass(IConfiguration configuration)
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

        protected virtual void Dispose(bool disposing)
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