using Drive.Data.DTOs;
using Drive.Helpers.JwtUtils;
using Drive.Data.Models;
using Drive.Data.Enums;
using Drive.UnitOfWork;
using BCryptNet = BCrypt.Net.BCrypt;
using Microsoft.AspNetCore.Identity;
using Drive.Data.DTOs.UserDto;

namespace Drive.Services.UserService
{
    public class UserService: IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtUtils _jwtUtils;

        public UserService(IUnitOfWork unitOfWork, IJwtUtils jwtUtils)
        {
            _unitOfWork = unitOfWork;
            _jwtUtils = jwtUtils;
        }

        public User GetById(Guid id)
        {
            return _unitOfWork.UserRepository.FindById(id);
        }

        public async Task<UserLoginResponseDto> Login(UserDto userDto)
        {
            var user = await _unitOfWork.UserRepository.FindByUsername(userDto.Username);

            if (user == null || !BCryptNet.Verify(userDto.Password, user.Password))
            {
                return null;
            }
            if (user == null) return null;

            var token = _jwtUtils.GenerateJwtToken(user);
            return new UserLoginResponseDto(user, token);
        }

        public async Task<bool> Register(UserDto userDto, Role userRole)
        {
            var userToCreate = new User
            {
                Username = userDto.Username,
                Role = userRole,
                Password = BCryptNet.HashPassword(userDto.Password)
            };

            _unitOfWork.UserRepository.Create(userToCreate);
            return await _unitOfWork.UserRepository.SaveAsync();
        }
    }
}
