using FinalTask.Application.Dtos;

namespace FinalTask.Application.Services.Contracts
{
    public interface IAuthService
    {
        Task<TokenModel> Login(LoginModel loginModel);
        Task Register(RegisterModel registerModel);
        Task RegisterAdmin(RegisterModel registerModel);
        Task<TokenModel> RefreshToken(TokenModel tokenModel);
        Task Revoke(string username);
        Task RevokeAll();
    }
}
