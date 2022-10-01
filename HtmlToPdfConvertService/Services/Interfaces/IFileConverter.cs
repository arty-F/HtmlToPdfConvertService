namespace HtmlToPdfConvertService.Services.Interfaces
{
    /// <summary>
    /// Provides functionality to convert file from to another type.
    /// </summary>
    public interface IFileConverter
    {
        /// <summary>
        /// Convert recieving file form content to new file type.
        /// </summary>
        /// <param name="fileForm">File form to convertation.</param>
        /// <returns>File data.</returns>
        public Task<byte[]> Convert(IFormFile fileForm);

        /// <summary>
        /// Is this file converter supports recieving file type convertation.
        /// </summary>
        /// <param name="fileForm">File form to convertation.</param>
        /// <returns>Convertation supporting.</returns>
        public bool IsSupportedExtension(IFormFile fileForm);

        /// <summary>
        /// Returns a new file name after convert operation.
        /// </summary>
        /// <param name="fileForm">File form to convertation.</param>
        /// <returns>File name after convertation.</returns>
        public string GetConvertedFileName(IFormFile fileForm);

        /// <summary>
        /// Returns content type of a converted file.
        /// </summary>
        /// <returns>File content type after convertation.</returns>
        public string GetConvertedFileType();
    }
}
