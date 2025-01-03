﻿namespace RekazTest.Models.PresentationModels
{
    public class BlobPresentationModel
    {
        public Guid Id { get; set; }
        public string Data { get; set; } = string.Empty;
        public int Size { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
