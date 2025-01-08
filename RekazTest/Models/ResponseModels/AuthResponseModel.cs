namespace RekazTest.Models.ResponseModels
{
    public class AuthResponseModel
    {
        public string token {  get; set; } = string.Empty;
        public bool IsAuthenticated { get; set; }
        public DateTime ExpiresOn { get; set; }
        public string message { get; set; } = string.Empty;
    }
}
