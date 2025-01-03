namespace RekazTest.Models.ResponseModels
{
    public class ResponseModel<T>
    {
        public ResponseModel(T response, string message = "")
        {
            Data = response;
            Message = message;
        }

        public T Data { get; set; }
        public string Message { get; set; }
    }
}
