using Microsoft.EntityFrameworkCore;
using RekazTest.Abstractions;
using RekazTest.DatabaseAccess;
using RekazTest.Models;
using RekazTest.Models.DTOs;
using RekazTest.Models.PresentationModels;
using System.Net.Sockets;

namespace RekazTest.Services
{
    public class MSSQLDatabase : IStorageBackend
    {
        private readonly ApplicationDbContext _context;
        public MSSQLDatabase(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<BlobPresentationModel> Get(Guid id)
        {
            try
            {
                var blob = await _context.Blobs.Where(b => b.Id == id).SingleAsync();

                if (blob != null)
                {
                    var blobMetadata = await _context.BlobsMetadata.Where(bm => bm.BlobId == id).SingleAsync();

                    var blobPresentationModel = new BlobPresentationModel
                    {
                        Id = blob.Id,
                        Data = blob.Data,
                        Size = blobMetadata.Size,
                        CreatedAt = blobMetadata.CreatedAt
                    };

                    return blobPresentationModel;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<BlobPresentationModel> Add(BlobAddDto dto) 
        {
            try 
            {
                var blob = new Blob
                {
                    Data = dto.Data
                };

                var blobMetadata = new BlobMetadata
                {
                    BlobId = blob.Id,
                    Size = dto.Size,
                    CreatedAt = dto.CreatedAt
                };

                var blobPresentationModel = new BlobPresentationModel
                {
                    Id = blob.Id,
                    Data = blob.Data,
                    Size = blobMetadata.Size,
                    CreatedAt = blobMetadata.CreatedAt
                };

                await _context.Blobs.AddAsync(blob);
                await _context.BlobsMetadata.AddAsync(blobMetadata);
                _context.SaveChanges();

                return blobPresentationModel;
            }
            catch
            {
                throw;
            }
        }

        public async Task<BlobPresentationModel> Delete(Guid id)
        {
            try
            {
                var blob = await _context.Blobs.Where(b => b.Id == id).SingleAsync();
                var blobMetadata = await _context.BlobsMetadata.Where(bm => bm.BlobId == id).SingleAsync();

                _context.Blobs.Remove(blob);
                _context.SaveChanges();

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

    }
}
