using RekazTest.Models.ResponseModels;

namespace RekazTest.Security
{
    public interface IAuth
    {
        AuthResponseModel AuthorizeRequest();
    }
}
