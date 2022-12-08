using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FinalTask.NET6._0.Services.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;

namespace FinalTask.UnitTests
{
    [TestClass]
    public class TokenServiceTests
    {
        private TokenService _tokenService;
        private Mock<IConfiguration> _configuration;
        private Mock<DateTime> datetime;

        public TokenServiceTests()
        {
            _configuration = new Mock<IConfiguration>();
        }

        [TestMethod]
        public async Task TokenService_CreateToken_ShouldReturnToken()
        {
            var authClaims = new List<Claim>
            {
                    new Claim(ClaimTypes.Name, "Name"),
                    new Claim(JwtRegisteredClaimNames.Jti, "29ec3314-c9c1-4b42-a478-4a44004f9967"),
                    new Claim(ClaimTypes.Role, "User")
            };

            _configuration.Setup(con => con["JWT:Secret"]).Returns("MySeCrEtKey93333333333333213123");
            _configuration.Setup(con => con["JWT:ValidIssuer"]).Returns("http://localhost:5000");
            _configuration.Setup(con => con["JWT:ValidAudience"]).Returns("http://localhost:4200");

            _tokenService = new TokenService(_configuration.Object);

            //var result = service.CreateToken(authClaims);
            
        }
    }
}
