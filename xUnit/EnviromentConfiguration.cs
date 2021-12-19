using System;

namespace xUnitTests
{
    class EnviromentConfiguration : IConfiguration
    {
        string _browserVariableName = "BROWSER";
        string _headlessVariableName = "HEADLESS";
        string _slowMoVariableName = "SLOWMO";


        private string GetValueOr(string name, string coerce)
            => Environment.GetEnvironmentVariable(name) ?? coerce;

        public string BrowserName => GetValueOr(_browserVariableName, Microsoft.Playwright.BrowserType.Chromium).ToLower();

        public bool Headless => bool.TryParse(GetValueOr(_headlessVariableName, bool.TrueString), out var res) ? res : true;

        public int SlowMo => int.TryParse(GetValueOr(_slowMoVariableName, 0.ToString()), out var res) ? res : 0;

    }
}