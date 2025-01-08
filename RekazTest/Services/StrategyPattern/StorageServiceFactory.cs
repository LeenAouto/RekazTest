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
                    configuration["LocalFileSystemPaths:BlobsStoragePath"] ?? throw new ArgumentNullException("BlobsStoragePath is missing in configuration."),
                    configuration["LocalFileSystemPaths:BlobsMetadataStoragePath"] ?? throw new ArgumentNullException("BlobsMetadataStoragePath is missing in configuration.")
                    ),
                "AmazonS3" => new AmazonS3(
                    configuration["S3Options:BlobsBucketName"] ?? throw new ArgumentNullException("BucketName is missing in configuration."),
                    configuration["S3Options:Region"] ?? throw new ArgumentNullException("Region is missing in configuration."),
                    configuration["S3Options:AccessKey"] ?? throw new ArgumentNullException("AccessKey is missing in configuration."),
                    configuration["S3Options:SecretKey"] ?? throw new ArgumentNullException("SecretKey is missing in configuration.")
                    ),
                //"FTP" => new FTP(configuration),
                _ => throw new ArgumentException("Invalid backend type")
            };
        }
    }
}
