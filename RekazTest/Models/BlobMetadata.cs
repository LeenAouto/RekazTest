namespace RekazTest.Models
{
    public class BlobMetadata
    {
        public Guid Id { get; set; }
        public Guid BlobId { get; set; }
        public int Size { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? BackendType { get; set; }
        public Blob? Blob { get; set; }
    }
}
