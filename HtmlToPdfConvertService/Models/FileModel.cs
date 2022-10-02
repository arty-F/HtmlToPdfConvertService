using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace HtmlToPdfConvertService.Models
{
    public class FileModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Name { get; set; } = null!;

        public string Md5 { get; set; } = null!;

        public byte[] ConvertedData { get; set; } = null!;
    }
}
