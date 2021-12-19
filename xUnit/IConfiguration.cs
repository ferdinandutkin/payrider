namespace xUnitTests
{

    public interface IConfiguration
    {
        public string BrowserName { get; }

        public bool Headless { get; }

        public int SlowMo { get; }
    }

}