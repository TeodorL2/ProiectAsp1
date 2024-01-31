using Drive.Data.Models;

namespace Drive.Helpers.JwtUtils
{
    public interface IJwtUtils
    {
        string GenerateJwtToken(User user);
        Guid? GetUserId(string? token);
    }
}
