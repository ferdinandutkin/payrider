using Microsoft.Extensions.DependencyInjection;
using Shared;
using System;
using System.IO;
using xUnitTests.Framework;

namespace xUnitTests;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IConfiguration, EnviromentConfiguration>();
        services.AddSingleton<IPlaywrightFixture, PlaywrightFixture>();

        services.AddSingleton<ICaptchaBypassUrlProvider, CaptchaBypassUrlProvider>();
        services.AddSingleton<ICredentialsProvider, JsonCredentialsProvider>(_ => new JsonCredentialsProvider(Path
            .GetFullPath(Path
                .Combine(Directory.GetCurrentDirectory(), @"..\..\..\credentials.json"))));
    }
}