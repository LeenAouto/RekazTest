namespace RekazTest.Models.DTOs
{
    public class BlobAddDto
    {
        public string Data { get; set; } = string.Empty;
        public int Size { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
