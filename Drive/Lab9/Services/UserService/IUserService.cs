using Drive.Data.DTOs;
using Drive.Data.Models;
using Drive.Data.Enums;
using Drive.Data.DTOs.UserDto;

namespace Drive.Services.UserService
{
    public interface IUserService
    {
        Task<UserLoginResponseDto> Login(UserDto user);

        User GetById(Guid id);

        Task<bool> Register(UserDto userRegisterDto, Role userRole);
    }
}
