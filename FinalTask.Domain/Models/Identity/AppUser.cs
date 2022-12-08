using Microsoft.AspNetCore.Identity;

namespace FinalTask.Domain.Models.Identity
{
    public class AppUser : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
