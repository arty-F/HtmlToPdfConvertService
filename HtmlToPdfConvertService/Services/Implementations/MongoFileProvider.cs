using HtmlToPdfConvertService.Models;
using HtmlToPdfConvertService.Services.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System.Security.Cryptography;

namespace HtmlToPdfConvertService.Services.Implementations
{
    public class MongoFileProvider : IFileProvider
    {
        private readonly GridFSBucket bucket;

        public MongoFileProvider(IOptions<FilesDatabaseSettings> filesDatabaseSettings)
        {
            var mongoClient = new MongoClient(filesDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(filesDatabaseSettings.Value.DatabaseName);
            bucket = new GridFSBucket(mongoDatabase);
        }

        public async Task CreateAsync(IFormFile fileForm)
        {
            await bucket.UploadFromStreamAsync(fileForm.FileName, fileForm.OpenReadStream());
        }

        public async Task<FileModel?> GetAsync(IFormFile fileForm)
        {
            var fileInfo = await Find(fileForm);
            if (fileInfo == null)
            {
                return null;
            }
            var bytes = await bucket.DownloadAsBytesAsync(fileInfo.Id);
            return new FileModel() { Data = bytes };
        }

        private async Task<GridFSFileInfo?> Find(IFormFile fileForm)
        {
            var md5 = await GetFileMD5(fileForm);
            var filter = Builders<GridFSFileInfo>.Filter.And(
                Builders<GridFSFileInfo>.Filter.Eq(x => x.Filename, fileForm.FileName),
                Builders<GridFSFileInfo>.Filter.Eq(x => x.MD5, md5));

            using var cursor = await bucket.FindAsync(filter, new GridFSFindOptions { Limit = 1 });
            return (await cursor.ToListAsync()).FirstOrDefault();
        }

        private async Task<string> GetFileMD5(IFormFile fileForm)
        {
            using var md5 = MD5.Create();
            var bytes = await Task.Run(() => md5.ComputeHash(fileForm.OpenReadStream()));
            return Convert.ToHexString(bytes).ToLower();
        }
    }
}
