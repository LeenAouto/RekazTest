using RekazTest.Models.DTOs;
using RekazTest.Models.PresentationModels;

namespace RekazTest.Abstractions
{
    public interface IStorageBackend
    {
        Task<BlobPresentationModel> Get(Guid id);
        Task<BlobPresentationModel> Add(BlobAddDto dto);
        Task<BlobPresentationModel> Delete(Guid id);
    }
}
