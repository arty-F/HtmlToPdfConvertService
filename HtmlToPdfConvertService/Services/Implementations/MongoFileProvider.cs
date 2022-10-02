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
        private readonly IMongoCollection<FileModel> filesCollection;

        public MongoFileProvider(IOptions<FilesDatabaseSettings> filesDatabaseSettings)
        {
            var mongoClient = new MongoClient(filesDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(filesDatabaseSettings.Value.DatabaseName);
            filesCollection = mongoDatabase.GetCollection<FileModel>(filesDatabaseSettings.Value.CollectionName);
        }

        public async Task<FileModel?> GetAsync(IFormFile fileForm)
        {
            var md5 = await GetFileMD5Async(fileForm);
            return await filesCollection
                .Find(f => f.Name == fileForm.FileName && f.Md5 == md5)
                .FirstOrDefaultAsync();
        }

        public async Task CreateAsync(IFormFile fileForm, byte[] convertedData)
        {
            var newFile = new FileModel
            {
                Name = fileForm.FileName,
                Md5 = await GetFileMD5Async(fileForm),
                ConvertedData = convertedData
            };
            await filesCollection.InsertOneAsync(newFile);
        }

        private async Task<string> GetFileMD5Async(IFormFile fileForm)
        {
            using var md5 = MD5.Create();
            var bytes = await Task.Run(() => md5.ComputeHash(fileForm.OpenReadStream()));
            return Convert.ToHexString(bytes).ToLower();
        }
    }
}
