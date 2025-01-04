using RekazTest.Abstractions;
using RekazTest.DatabaseAccess;

namespace RekazTest.Services.StrategyPattern
{
    public class StorageServiceFactory
    {
        public static IStorageBackend Create(string backendType, IConfiguration configuration, ApplicationDbContext dbContext)
        {
            return backendType switch
            {
                "Database" => new MSSQLDatabase(dbContext),
                "LocalFileSystem" => new LocalFileSystem(
                    configuration["BlobsStoragePath"] ?? throw new ArgumentNullException("BlobsStoragePath is missing in configuration."),
                    configuration["BlobsMetadataStoragePath"] ?? throw new ArgumentNullException("BlobsMetadataStoragePath is missing in configuration.")
                    ),
                //"AmazonS3" => new AmazonS3(configuration),
                //"FTP" => new FTP(configuration),
                _ => throw new ArgumentException("Invalid backend type")
            };
        }
    }
}
