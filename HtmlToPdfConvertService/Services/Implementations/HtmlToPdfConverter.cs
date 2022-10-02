using HtmlToPdfConvertService.Services.Interfaces;
using PuppeteerSharp;
using System.Text;

namespace HtmlToPdfConvertService.Services.Implementations
{
    public class HtmlToPdfConverter : IFileConverter
    {
        private const string INITIAL_EXTENSION = ".html";
        private const string RESULT_EXTENSION = ".pdf";
        private const string CONVERTED_FILE_TYPE = "application/pdf";

        private readonly IFileProvider fileProvider;

        public HtmlToPdfConverter(IFileProvider fileProvider)
        {
            this.fileProvider = fileProvider;
        }

        public async Task<byte[]> Convert(IFormFile fileForm)
        {
            var file = await fileProvider.GetAsync(fileForm);
            if (file != null)
            {
                return file.ConvertedData;
            }

            var convertedData = await GetBytes(fileForm);
            await fileProvider.CreateAsync(fileForm, convertedData);
            return convertedData;
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

            var fileData = new StringBuilder();
            using var reader = new StreamReader(fileForm.OpenReadStream());
            while (reader.Peek() >= 0)
            {
                fileData.AppendLine(reader.ReadLine());
            }
                
            await page.SetContentAsync(fileData.ToString());
            return await page.PdfDataAsync();
        }
    }
}
