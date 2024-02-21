using System.ComponentModel.DataAnnotations;

namespace Drive.Data.DTOs
{
    public class UserRegisterRequestDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
