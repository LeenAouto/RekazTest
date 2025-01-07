using RekazTest.Abstractions;
using RekazTest.Models.DTOs;
using RekazTest.Models.PresentationModels;
using RekazTest.Models;
using System.Text;
using System.Text.Json;
using System.Security.Cryptography;

namespace RekazTest.Services
{
    public class AmazonS3 : IStorageBackend
    {
        private readonly string _blobsBucketName;
        private readonly string _region;
        private readonly string _accessKey;
        private readonly string _secretKey;
        private readonly HttpClient _httpClient;

        public AmazonS3(string blobsBucketName, string region, string accessKey, string secretKey)
        {
            _blobsBucketName = blobsBucketName;
            _region = region;
            _accessKey = accessKey;
            _secretKey = secretKey;
            _httpClient = new HttpClient();
        }
        public async Task<BlobPresentationModel?> Get(Guid id)
        {
            try
            {
                var blobResponse = await SendS3Request("GET", $"Blobs/{id}");
                var metadataResponse = await SendS3Request("GET", $"BlobsMetadata/meta_{id}");

                if (!blobResponse.IsSuccessStatusCode || !metadataResponse.IsSuccessStatusCode)
                    return null;

                var blobData = await blobResponse.Content.ReadAsStringAsync();
                var metadataJson = await metadataResponse.Content.ReadAsStringAsync();

                var blob = JsonSerializer.Deserialize<Blob>(blobData);
                var metadata = JsonSerializer.Deserialize<BlobMetadata>(metadataJson);

                var blobPresentationModel = new BlobPresentationModel
                {
                    Id = id,
                    Data = blob?.Data ?? "Couldn't get the data",
                    Size = metadata?.Size ?? 0,
                    CreatedAt = metadata?.CreatedAt ?? DateTime.UtcNow
                };

                return blobPresentationModel;
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

                var metadata = new BlobMetadata
                {
                    Id = Guid.NewGuid(),
                    BlobId = blob.Id,
                    Size = dto.Size,
                    CreatedAt = dto.CreatedAt
                };

                var blobContent = new StringContent(JsonSerializer.Serialize(blob), Encoding.UTF8, "application/json");
                var metadataContent = new StringContent(JsonSerializer.Serialize(metadata), Encoding.UTF8, "application/json");

                var blobResponse = await SendS3Request("PUT", $"Blobs/{blob.Id}", blobContent);
                var metadataResponse = await SendS3Request("PUT", $"BlobsMetadata/meta_{blob.Id}", metadataContent);

                if (!blobResponse.IsSuccessStatusCode || !metadataResponse.IsSuccessStatusCode)
                    return null;

                var blobPresentationModel = new BlobPresentationModel
                {
                    Id = blob.Id,
                    Data = blob.Data,
                    Size = metadata.Size,
                    CreatedAt = metadata.CreatedAt
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
                var blobResponse = await SendS3Request("DELETE", $"Blobs/{id}");
                var metadataResponse = await SendS3Request("DELETE", $"BlobsMetadata/meta_{id}");

                if (!blobResponse.IsSuccessStatusCode || !metadataResponse.IsSuccessStatusCode)
                    return null;


                var blobPresentationModel = new BlobPresentationModel
                {
                    Id = id,
                    Data = "File is Seleted successfully.",
                    Size = 0,
                    CreatedAt = DateTime.UtcNow
                };

                return blobPresentationModel;
            }
            catch
            {
                throw;
            }
        }

        private async Task<HttpResponseMessage> SendS3Request(string method, string resourcePath, HttpContent? content = null)
        {
            try
            {
                var request = new HttpRequestMessage(new HttpMethod(method), $"https://{_blobsBucketName}.s3.{_region}.amazonaws.com/{resourcePath}");

                if (content != null)
                {
                    request.Content = content;
                }

                var date = DateTime.UtcNow.ToString("yyyyMMddTHHmmssZ");
                var dateShort = DateTime.UtcNow.ToString("yyyyMMdd");
                var host = $"{_blobsBucketName}.s3.{_region}.amazonaws.com";
                var payloadHash = content != null ? HashSHA256(await content.ReadAsStringAsync())
                                                    : "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";

                request.Headers.Add("Host", host);
                request.Headers.Add("x-amz-date", date);
                request.Headers.Add("x-amz-content-sha256", payloadHash);

                var canonicalUri = resourcePath.StartsWith("/") ? resourcePath : $"/{resourcePath}";

                var canonicalRequest = $"{method}\n" +
                       $"{canonicalUri}\n" + 
                       $"\n" +
                       $"host:{host}\n" +
                       $"x-amz-content-sha256:{payloadHash}\n" +
                       $"x-amz-date:{date}\n" +
                       $"\n" +
                       $"host;x-amz-content-sha256;x-amz-date\n" +
                       $"{payloadHash}";

                var stringToSign = $"AWS4-HMAC-SHA256\n" +
                                   $"{date}\n" +
                                   $"{dateShort}/{_region}/s3/aws4_request\n" +
                                   $"{HashSHA256(canonicalRequest)}";

                var signingKey = GetSignatureKey(_secretKey, dateShort, _region, "s3");
                var signature = BitConverter.ToString(HMACSHA256(stringToSign, signingKey)).Replace("-", "").ToLower();

                var authorizationHeader = $"AWS4-HMAC-SHA256 " +
                                          $"Credential={_accessKey}/{dateShort}/{_region}/s3/aws4_request, " +
                                          $"SignedHeaders=host;x-amz-content-sha256;x-amz-date, " +
                                          $"Signature={signature}";

                authorizationHeader = authorizationHeader.Trim();

                bool isAdded = request.Headers.TryAddWithoutValidation("Authorization", authorizationHeader);

                return await _httpClient.SendAsync(request);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while sending the S3 request", ex);
            }

        }

        private static string HashSHA256(string data)
        {
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        private static byte[] HMACSHA256(string data, byte[] key)
        {
            using var hmac = new HMACSHA256(key);
            return hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        }

        private static byte[] GetSignatureKey(string key, string dateStamp, string regionName, string serviceName)
        {
            var kDate = HMACSHA256(dateStamp, Encoding.UTF8.GetBytes("AWS4" + key));
            var kRegion = HMACSHA256(regionName, kDate);
            var kService = HMACSHA256(serviceName, kRegion);
            var kSigning = HMACSHA256("aws4_request", kService);
            return kSigning;
        }
    }
}
