using System.ComponentModel.DataAnnotations;

namespace Drive.Data.DTOs.UserDto
{
    public class UserDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
