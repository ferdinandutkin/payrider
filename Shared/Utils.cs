using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public static class Utils
    {
        public static string GenerateFileName(string testName, string browserType, string extension)
        {
            string os =
                OperatingSystem.IsLinux() ? "linux" :
                OperatingSystem.IsMacOS() ? "macos" :
                OperatingSystem.IsWindows() ? "windows" :
                "other";

            // Remove characters that are disallowed in file names
            browserType = browserType.Replace(':', '_');

            string utcNow = DateTimeOffset.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture);
            return $"{testName}_{browserType}_{os}_{utcNow}{extension}";
        }
    }
}
