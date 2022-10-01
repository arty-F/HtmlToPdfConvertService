using HtmlToPdfConvertService.Models;

namespace HtmlToPdfConvertService.Services.Interfaces
{
    public interface IFileProvider
    {
        public Task<FileModel?> GetAsync(IFormFile fileForm);
        public Task CreateAsync(IFormFile fileForm);
    }
}
