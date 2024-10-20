using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace PdfGenerator
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            // Setup item and cookie parameters here
            var item = new Item
            {
                Name = "Sample Form",
                Href =
                    "https://devblog.maheshyadav.com.np/development%20tutorials/programming%20techniques/software%20engineering/Seamless-PDF-Generation-with-Puppeteer-in-C-Sharp/"
            };
            string docPhysicalPath = item.Name + ".pdf";
            // Execute the PDF generation
            bool result = await DownloadFormToPdf(item, docPhysicalPath);

            Console.WriteLine(result ? "PDF generated successfully!" : "PDF generation failed.");
        }

        internal static async Task<bool> DownloadFormToPdf(Item item, string docPhysicalPath, int retryCount = 10)
        {
            if (retryCount <= 0)
            {
                throw new Exception("Max retry count exceeded.");
            }

            var apiRetryAttempts = 10 - retryCount + 1; // Assuming RetryAttempts = 10
            Log.WriteLine(LogLevel.Normal, "Downloading Form '{0}' with Url: {1} at attempt {2}", item.Name, item.Href,
                apiRetryAttempts);

            try
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();

                var authenticator = new SeleniumAuthenticator();
                var launchOptions = new LaunchOptions
                {
                    Headless = true,
                    ExecutablePath = authenticator.GetInstalledBrowserPath("chrome"),
                    IgnoreHTTPSErrors = true
                };

                using (var browser = await Puppeteer.LaunchAsync(launchOptions))
                using (var page = await browser.NewPageAsync())
                {
                    //await page.SetCookieAsync(cookieParam);
                    await page.SetUserAgentAsync(
                        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

                    var response = await page.GoToAsync(item.Href, WaitUntilNavigation.DOMContentLoaded);
                    if (response.Status == HttpStatusCode.OK)
                    {
                        var pdfOptions = new PdfOptions
                        {
                            PrintBackground = true,
                            Format = PaperFormat.A4,
                            MarginOptions = new MarginOptions
                                { Bottom = "0px", Left = "0px", Right = "0px", Top = "0px" },
                            Scale = 1m
                        };

                        await page.PdfAsync(docPhysicalPath, pdfOptions);
                        watch.Stop();
                        var file = new FileInfo(docPhysicalPath);
                        Log.WriteLine(LogLevel.Normal, "Download success for Form '{0}' in {1} ms", file.Name,
                            watch.ElapsedMilliseconds);
                        return true;
                    }

                    Log.WriteLine(LogLevel.Low,
                        "Failed Downloading Form '{0}' with url: '{1}' at attempt {2}, Response Status: {3}.",
                        item.Name, item.Href, apiRetryAttempts, response.Status);
                    retryCount--;

                    // Retry logic
                    if (response.Status == HttpStatusCode.ServiceUnavailable)
                    {
                        await Task.Delay(3000); // Delay before retry
                        return await DownloadFormToPdf(item, docPhysicalPath, retryCount);
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.WriteLine(LogLevel.Low, "Failed Downloading Form '{0}' with url: '{1}'. Exception: {2}", item.Name,
                    item.Href, ex.Message);
                return false;
            }
        }
    }

    // Placeholder classes for the example
    public class Item
    {
        public string Name { get; set; }
        public string Href { get; set; }
    }

    public class SeleniumAuthenticator
    {
        public string GetInstalledBrowserPath(string browser)
        {
            // Logic to return the path of the installed browser (e.g., Chrome)
            return @"C:\Program Files\Google\Chrome\Application\chrome.exe"; // Adjust path as needed
        }
    }

    // Mocking Log class for logging
    public static class Log
    {
        public static void WriteLine(LogLevel level, string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }
    }

    public enum LogLevel
    {
        Normal,
        Low,
        High
    }
}