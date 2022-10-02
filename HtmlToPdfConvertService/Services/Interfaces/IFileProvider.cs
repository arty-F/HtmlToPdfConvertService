using HtmlToPdfConvertService.Models;

namespace HtmlToPdfConvertService.Services.Interfaces
{
    /// <summary>
    /// Provides a functionality of file storage operations.
    /// </summary>
    public interface IFileProvider
    {
        /// <summary>
        /// Find and get file from inner storage.
        /// </summary>
        /// <param name="fileForm">Searched file.</param>
        /// <returns>Finded file or null if not exist.</returns>
        public Task<FileModel?> GetAsync(IFormFile fileForm);

        /// <summary>
        /// Creating a new file in inner storage.
        /// </summary>
        /// <param name="fileForm">Initial file form.</param>
        /// <param name="convertedData">Converted file data.</param>
        public Task CreateAsync(IFormFile fileForm, byte[] convertedData);
    }
}
