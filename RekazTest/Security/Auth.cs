using Microsoft.IdentityModel.Tokens;
using RekazTest.Models.ResponseModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RekazTest.Security
{
    public class Auth : IAuth
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly double _durationInMinutes;


        public Auth(IConfiguration configuration)
        {
            _key = configuration["JWT:Key"] ?? throw new ArgumentNullException("JWT Key is not defined.");
            _issuer = configuration["JWT:Issuer"] ?? throw new ArgumentNullException("JWT Issuer is not defined.");
            _audience = configuration["JWT:Audience"] ?? throw new ArgumentNullException("JWT Audience is not defined.");
            _durationInMinutes = Convert.ToDouble(configuration["JWT:DurationInMinutes"]);
        }

        public AuthResponseModel AuthorizeRequest()
        {
            try
            {
                var jwtToken = CreateJwtToken();

                var authResponseModel = new AuthResponseModel
                {
                    token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    IsAuthenticated = true,
                    ExpiresOn = jwtToken.ValidTo
                };

                return authResponseModel;
            }
            catch 
            {
                throw;
            }

        }

        private JwtSecurityToken CreateJwtToken()
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                expires: DateTime.Now.AddMinutes(_durationInMinutes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
    }
}
