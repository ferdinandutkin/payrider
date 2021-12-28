namespace xUnitTests
{
    internal class DebugConfiguration : IConfiguration
    {
        public string BrowserName => Microsoft.Playwright.BrowserType.Chromium;
        public bool Headless => false;
        public int SlowMo => 500;
        public int DefaultTimeout => 100000;
    }
}
