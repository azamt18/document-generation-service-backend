using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace Service.Converter;

public static class ConverterService
{
    public static async Task ConvertHtmlToPdf(string inputFilePath, string outputFilePath)
    {
         var html = await System.IO.File.ReadAllTextAsync(inputFilePath);

        // download the browser executable
        await new BrowserFetcher().DownloadAsync();

        // browser execution configs
        var launchOptions = new LaunchOptions
        {
            Headless = true, // = false for testing
        };

        // open a new page in the controlled browser
        using (var browser = await Puppeteer.LaunchAsync(launchOptions))
        using (var page = await browser.NewPageAsync())
        {
            // scraping logic...
            await page.SetContentAsync(html);

            // Enabling links in the pdf generated
            var pdfOptions = new PdfOptions
            {
                DisplayHeaderFooter = false,
                Landscape = false,
                PrintBackground = false,
                Format = PaperFormat.A4,
                MarginOptions = new MarginOptions { Top = "1cm", Bottom = "1cm", Left = "1cm", Right = "1cm" },
                Scale = 1,
            };

            await page.PdfAsync(outputFilePath, pdfOptions);
        }

        //await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
        //await using var page = await browser.NewPageAsync();

        

        // var pdfOptions = new PuppeteerSharp.PdfOptions();
        // // pdfOptions.Format = PuppeteerSharp.PaperFormat.A4;
        //
        // await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        // {
        //     Headless = true,
        //     ExecutablePath = "CHROME_BROWSER_PATH"
        // });
        //
        // await using var page = await browser.NewPageAsync();
        // await page.SetContentAsync(html);
        // await page.PdfAsync(pdfFileName, pdfOptions);
    }
}