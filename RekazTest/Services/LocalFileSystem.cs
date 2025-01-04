using RekazTest.Abstractions;
using RekazTest.Models.DTOs;
using RekazTest.Models.PresentationModels;
using RekazTest.Models;
using System.Text.Json;

namespace RekazTest.Services
{
    public class LocalFileSystem : IStorageBackend
    {
        private readonly string _blobsStoragePath;
        private readonly string _blobsMetadataStoragePath;

        public LocalFileSystem(string blobsStoragePath, string blobsMetadataStoragePath)
        {
            if (string.IsNullOrWhiteSpace(blobsStoragePath) || string.IsNullOrWhiteSpace(blobsMetadataStoragePath))
                throw new ArgumentNullException(nameof(blobsStoragePath), "Storage path cannot be null or empty.");

            _blobsStoragePath = blobsStoragePath;
            _blobsMetadataStoragePath = blobsMetadataStoragePath;

            if (!Directory.Exists(_blobsStoragePath))
                Directory.CreateDirectory(_blobsStoragePath);

            if (!Directory.Exists(_blobsMetadataStoragePath))
                Directory.CreateDirectory(_blobsMetadataStoragePath);
        }

        public async Task<BlobPresentationModel?> Get(Guid id)
        {
            try
            {
                var (blob, metadata) = await GetBlobAndMetadata(id);

                if (blob != null && metadata != null)
                {
                    return new BlobPresentationModel
                    {
                        Id = blob.Id,
                        Data = blob.Data,
                        Size = metadata.Size,
                        CreatedAt = metadata.CreatedAt
                    };
                }

                return null;
            }
            catch
            {
                throw;
            }
        }

        public async Task<BlobPresentationModel?> Add(BlobAddDto dto)
        {
            try
            {
                var blob = new Blob
                {
                    Id = Guid.NewGuid(),
                    Data = dto.Data
                };

                var blobMetadata = new BlobMetadata
                {
                    Id = Guid.NewGuid(),
                    BlobId = blob.Id,
                    Size = dto.Size,
                    CreatedAt = dto.CreatedAt
                };

                var blobsFilePath = GetFilePath(blob);
                var blobsMetadataFilePath = GetFilePath(blobMetadata);

                var options = new JsonSerializerOptions { WriteIndented = true };

                string blobJsonString = JsonSerializer.Serialize(blob, options);
                string blobMetadataJsonString = JsonSerializer.Serialize(blobMetadata, options);

                await File.WriteAllTextAsync(blobsFilePath, blobJsonString);
                await File.WriteAllTextAsync(blobsMetadataFilePath, blobMetadataJsonString);

                var blobPresentationModel = new BlobPresentationModel
                {
                    Id = blob.Id,
                    Data = blob.Data,
                    Size = blobMetadata.Size,
                    CreatedAt = blobMetadata.CreatedAt
                };

                return blobPresentationModel;
            }
            catch
            {
                throw;
            }
        }

        public async Task<BlobPresentationModel?> Delete(Guid id)
        {
            try
            {
                var (blob, metadata) = await GetBlobAndMetadata(id);

                if (blob != null && metadata != null)
                {
                    File.Delete(GetFilePath(id, "blob"));
                    File.Delete(GetFilePath(id, "meta"));

                    return new BlobPresentationModel
                    {
                        Id = blob.Id,
                        Data = blob.Data,
                        Size = metadata.Size,
                        CreatedAt = metadata.CreatedAt
                    };
                }

                return null;
            }
            catch
            {
                throw;
            }
        }


        private string GetFilePath(Guid id, string folder)
        {
            try
            {
                return folder switch
                {
                    "blob" => Path.Combine(_blobsStoragePath, $"{id}.json"),
                    "meta" => Path.Combine(_blobsMetadataStoragePath, $"meta_{id}.json"),
                    _ => throw new ArgumentException("Invalid folder")
                };
            }
            catch
            {
                throw;
            }
        }
        private string GetFilePath(Blob blob)
        {
            try
            {
                return Path.Combine(_blobsStoragePath, $"{blob.Id}.json");
            }
            catch 
            {
                throw;
            }
        }
        private string GetFilePath(BlobMetadata blobMetadata)
        {
            try
            {
                return Path.Combine(_blobsMetadataStoragePath, $"meta_{blobMetadata.BlobId}.json");
            }
            catch
            {
                throw;
            }
        }


        private async Task<(Blob?, BlobMetadata?)> GetBlobAndMetadata(Guid id)
        {
            var blobFilePath = GetFilePath(id, "blob");
            var metadataFilePath = GetFilePath(id, "meta");

            if (!File.Exists(blobFilePath) || !File.Exists(metadataFilePath))
                return (null, null);

            var blob = JsonSerializer.Deserialize<Blob>(await File.ReadAllTextAsync(blobFilePath));
            var metadata = JsonSerializer.Deserialize<BlobMetadata>(await File.ReadAllTextAsync(metadataFilePath));

            return (blob, metadata);
        }
    }
}
