namespace RekazTest.Models
{
    public class BlobResponceModel
    {
        public Guid Id { get; set; }
        public byte[] Data { get; set; } = new byte[0];
        public int Size { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
