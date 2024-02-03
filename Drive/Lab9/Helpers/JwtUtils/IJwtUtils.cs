using Drive.Data.Models;

namespace Drive.Helpers.JwtUtil
{
    public interface IJwtUtils
    {
        string GenerateJwtToken(User user);
        Guid? GetUserId(string? token);
    }
}