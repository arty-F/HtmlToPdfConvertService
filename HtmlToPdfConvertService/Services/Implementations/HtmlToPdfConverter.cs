using HtmlToPdfConvertService.Services.Interfaces;
using PuppeteerSharp;

namespace HtmlToPdfConvertService.Services.Implementations
{
    public class HtmlToPdfConverter : IFileConverter
    {
        private const string INITIAL_EXTENSION = ".html";
        private const string RESULT_EXTENSION = ".pdf";
        private const string CONVERTED_FILE_TYPE = "application/pdf";

        public async Task<byte[]> Convert(IFormFile fileForm)
        {
            return await GetBytes(fileForm);
        }

        public bool IsSupportedExtension(IFormFile fileForm)
        {
            return Path.GetExtension(fileForm.FileName) == INITIAL_EXTENSION;
        }

        public string GetConvertedFileName(IFormFile fileForm)
        {
            return Path.GetFileNameWithoutExtension(fileForm.FileName) + RESULT_EXTENSION;
        }

        public string GetConvertedFileType()
        {
            return CONVERTED_FILE_TYPE;
        }

        private async Task<byte[]> GetBytes(IFormFile fileForm)
        {
            using var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
            await using var page = await browser.NewPageAsync();
            await page.SetContentAsync("<div>My Receipt</div>");
            var result = await page.PdfDataAsync();
            return await page.PdfDataAsync();
        }
    }
}
