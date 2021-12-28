using System;
using Microsoft.Playwright;

namespace xUnitTests;

public interface IPlaywrightFixture : IDisposable
{
    IBrowser Browser { get; }
    IPlaywright Playwright { get; }
    IBrowserType BrowserType { get; }
    IBrowserContext Context { get; }
    void Dispose(bool disposing);
    void Dispose();
}