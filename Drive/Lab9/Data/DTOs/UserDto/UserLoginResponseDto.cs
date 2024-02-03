using Drive.Data.Models;

namespace Drive.Data.DTOs.UserDto
{
    public class UserLoginResponseDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }

        public string Token { get; set; }

        public UserLoginResponseDto(User user, string token)
        {
            Id = user.Id;
            Username = user.Username;
            Token = token;
        }
    }
}
