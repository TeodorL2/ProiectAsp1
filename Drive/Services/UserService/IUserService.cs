using Drive.Data.DTOs;
using Drive.Data.Enums;
using Drive.Data.Models;

namespace Drive.Services.UserService
{
    public interface IUserService
    {
        Task<UserLoginResponseDto> Login(UserLoginRequestDto user);
        User GetById(Guid id);

        Task<UserLoginResponseDto> Register(UserRegisterRequestDto userRegisterDto, Role userRole);
    }
}
