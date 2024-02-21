using System.ComponentModel.DataAnnotations;

namespace Drive.Data.DTOs
{
    public class UserLoginRequestDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
