﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RekazTest.Abstractions;
using RekazTest.Models.DTOs;
using RekazTest.Models.PresentationModels;
using RekazTest.Models.ResponseModels;
using RekazTest.Services.StrategyPattern;

namespace RekazTest.Controllers
{
    //[Route("api/[controller]")]
    [Route("v1/blobs")]
    [ApiController]
    public class BlobController : ControllerBase
    {
        //private readonly IStorageBackend _storageBackend;
        private readonly StorageServiceContext _storageContext;

        public BlobController(StorageServiceContext storageContext)
        {
            _storageContext = storageContext;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlob(Guid id)
        {
            try
            {
                var blob = await _storageContext.GetBlob(id);

                if (blob == null)
                {
                    return NotFound();
                }

                return Ok(new ResponseModel<BlobPresentationModel>(blob, "Blob is found"));
            }
            catch
            {
                return BadRequest("An error occured");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadBlob([FromBody] BlobInputDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Data))
                    return BadRequest("Input data is required");

                if (!IsValidBase64(dto.Data))
                    return BadRequest("Input string cannot be decoded");

                var blobAdd = new BlobAddDto
                {
                    Data = dto.Data,
                    Size = GetBlobDataSizeInBytes(dto.Data),
                    CreatedAt = DateTime.UtcNow
                };

                var blob = await _storageContext.AddBlob(blobAdd);

                if (blob == null)
                    return BadRequest("Something went wrong");

                return Ok(new ResponseModel<BlobPresentationModel>(blob, "Blob is saved"));

            }
            catch
            {
                return BadRequest("An error occured");
            }
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlob(Guid id)
        {
            try
            {
                var blob = await _storageContext.DeleteBlob(id);

                if (blob == null)
                {
                    return NotFound();
                }

                return Ok(new ResponseModel<BlobPresentationModel>(blob, "Blob is deleted"));
            }
            catch (Exception)
            {

                throw;
            }
        }

        [NonAction]
        private bool IsValidBase64(string base64String)
        {
            try
            {
                if (string.IsNullOrEmpty(base64String))
                    return false;

                Convert.FromBase64String(base64String);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        [NonAction]
        private int GetBlobDataSizeInBytes(string data)
        {
            try
            {
                int padding = data.EndsWith("==") ? 2 : data.EndsWith("=") ? 1 : 0;
                int dataSize = (data.Length * 3 / 4) - padding;

                return dataSize;
            }
            catch
            {
                throw;
            }
        }
    }

}
