using RekazTest.Abstractions;
using RekazTest.Models.DTOs;
using RekazTest.Models.PresentationModels;

namespace RekazTest.Services.StrategyPattern
{
    public class StorageServiceContext
    {
        private readonly IStorageBackend _storageBackend;

        public StorageServiceContext(IStorageBackend storageBackend)
        {
            _storageBackend = storageBackend;
        }

        public Task<BlobPresentationModel?> GetBlob(Guid id) => _storageBackend.Get(id);
        public Task<BlobPresentationModel?> AddBlob(BlobAddDto dto) => _storageBackend.Add(dto);
        public Task<BlobPresentationModel?> DeleteBlob(Guid id) => _storageBackend.Delete(id);
    }
}
