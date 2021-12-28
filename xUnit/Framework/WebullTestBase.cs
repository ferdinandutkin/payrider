using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Playwright;
using PageObjects;
using Shared;
using Serilog;
using Serilog.Events;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;
using xUnitTests.Framework;


namespace xUnitTests
{
    public class WebullTestBase : XunitContextBase, IClassFixture<PlaywrightFixture>
    {
        protected PageObject PageUnderTest { get; set; }
        public readonly IBrowserContext _context;
        private readonly ICaptchaBypassUrlProvider _captchaBypassUrlProvider;


        protected ILogger Logger { get; }
        public CenterPage CenterPage { get; set; }

        protected string TestName { get; }
        protected string BrowserName { get; }






        public WebullTestBase(PlaywrightFixture fixture, ICredentialsProvider credentialsProvider, ICaptchaBypassUrlProvider captchaBypassUrlProvider, ITestOutputHelper output, [CallerFilePath] string sourceFile = "")
        : base(output, sourceFile)
        {

            TestName = Context.Test.DisplayName;

            BrowserName = fixture.BrowserType.Name;

            Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.TestOutput(output, LogEventLevel.Verbose, "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] " + TestName + " {Message}{NewLine}{Exception}")
                .CreateLogger();

            _captchaBypassUrlProvider = captchaBypassUrlProvider;
            _context = fixture.Context;

            var page = _context.NewPageAsync().Result;
            var loginPage = new HomePage(page)
                .ClickLoginAsync().Result;

            var (email, password) = credentialsProvider.Credentials;
            CenterPage = loginPage.LoginViaEmailAsync(email, password, _captchaBypassUrlProvider).Result;

        }


        private async Task TryCaptureScreenshotAsync(
            IPage page)
        {
            try
            {
                string fileName = Utils.GenerateFileName(TestName, BrowserName, ".png");
                string path = Path.Combine("screenshots", fileName);

                await page.ScreenshotAsync(new() { Path = path });

                Logger.Information($"Saved screenshot as {path}");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to capture screenshot");
            }
        }

        public override void Dispose()
        {
            var theExceptionThrownByTest = Context.TestException;

            if (theExceptionThrownByTest is null)
            {
                TryCaptureScreenshotAsync(PageUnderTest.Page).Wait();
            }
            base.Dispose();
        }



    }
}

