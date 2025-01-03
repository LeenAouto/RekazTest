//using Microsoft.EntityFrameworkCore;
//using RekazTest.Abstractions;
//using RekazTest.Models.DTOs;
//using RekazTest.Models.PresentationModels;
//using RekazTest.Models;

//namespace RekazTest.Services
//{
//    public class LocalFileSystem : IStorageBackend
//    {
//        private readonly string _blobsStoragePath;
//        private readonly string _blobsMetadataStoragePath;

//        public LocalFileSystem()
//        {
            
//        }
//        public async Task<BlobPresentationModel> Get(Guid id)
//        {
//            try
//            {
                
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        public async Task<BlobPresentationModel> Add(BlobAddDto dto)
//        {
//            try
//            {
//                var blob = new Blob
//                {
//                    Data = dto.Data
//                };

//                var blobMetadata = new BlobMetadata
//                {
//                    BlobId = blob.Id,
//                    Size = dto.Size,
//                    CreatedAt = dto.CreatedAt,
//                    BackendType = dto.BackendType
//                };

//                var blobPresentationModel = new BlobPresentationModel
//                {
//                    Id = blob.Id,
//                    Data = blob.Data,
//                    Size = blobMetadata.Size,
//                    CreatedAt = blobMetadata.CreatedAt
//                };

//                await File.WriteAllBytesAsync(filePath, data);

//                return blobPresentationModel;
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        public async Task<BlobPresentationModel> Delete(Guid id)
//        {
//            try
//            {
                
//            }
//            catch
//            {
//                throw;
//            }
//        }
//    }
//}
