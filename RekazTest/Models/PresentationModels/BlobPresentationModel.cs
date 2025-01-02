namespace RekazTest.Models.PresentationModels
{
    public class BlobPresentationModel
    {
        public BlobPresentationModel() { }
        public BlobPresentationModel(Blob blob)
        {
            Id = blob.Id;
            Data = blob.Data;
            Size = blob.Size;
            CreatedAt = blob.CreatedAt;
        }
        public Guid Id { get; set; }
        public byte[] Data { get; set; }
        public int Size { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
