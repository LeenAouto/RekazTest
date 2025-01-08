namespace RekazTest.Models.ResponseModels
{
    public class AuthResponseModel
    {
        public string Token {  get; set; } = string.Empty;
        public bool IsAuthenticated { get; set; }
        public DateTime ExpiresOn { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
