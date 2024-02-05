using Drive.Data.DTOs;
using Drive.Data.Models;
using Drive.Data.Enums;
using Drive.Data.DTOs.UserDto;
using Drive2.Data.DTOs.BaseDirectoryDto;
using Drive2.Data.DTOs;

namespace Drive.Services.UserService
{
    public interface IUserService
    {
        Task<UserLoginResponseDto> Login(UserDto user);

        User GetById(Guid id);

        Task<bool> Register(UserDto userRegisterDto, Role userRole);

        Task<List<BaseDirectoryDto>> GetAllWithPermission(Guid UserId);

    }
}
