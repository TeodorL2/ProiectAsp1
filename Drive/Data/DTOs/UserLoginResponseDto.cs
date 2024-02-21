using Drive.Data.Models;

namespace Drive.Data.DTOs
{
    public class UserLoginResponseDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }


        public UserLoginResponseDto(User user, string token)
        {
            Id = user.Id;
            UserName = user.UserName;
            Token = token;
        }
    }
}
