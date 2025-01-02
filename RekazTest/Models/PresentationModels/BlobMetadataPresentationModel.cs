namespace RekazTest.Models.PresentationModels
{
    public class BlobMetadataPresentationModel
    {
        public BlobMetadataPresentationModel() { }
        public BlobMetadataPresentationModel(BlobMetadata blobMetadata)
        {
            Id = blobMetadata.Id;
            BlobId = blobMetadata.BlobId;
            Size = blobMetadata.Size;
            CreatedAt = blobMetadata.CreatedAt;
            BackendType = blobMetadata.BackendType;
        }
        public Guid Id { get; set; }
        public Guid BlobId { get; set; }
        public int Size { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? BackendType { get; set; }
    }
}
