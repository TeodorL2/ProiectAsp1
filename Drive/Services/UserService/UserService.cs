using Drive.Data.DTOs;
using Drive.Data.Enums;
using Drive.Data.Exceptions;
using Drive.Data.Models;
using Drive.Helpers.JwtUtils;
using Drive.Repositories.UserRepository;
using Drive.UnitOfWork;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Drive.Services.UserService
{
    public class UserService : IUserService
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

        public async Task<UserLoginResponseDto> Login(UserLoginRequestDto userDto)
        {
            var user = _unitOfWork.UserRepository.FindByUserName(userDto.UserName);

            if (user == null || !BCryptNet.Verify(userDto.Password, user.Password))
            {
                return null; // or throw exception
            }
            if (user == null) return null;

            var token = _jwtUtils.GenerateJwtToken(user);
            return new UserLoginResponseDto(user, token);
        }

        public async Task<UserLoginResponseDto> Register(UserRegisterRequestDto userRegisterDto, Role userRole)
        {
            var userToCreate = new User
            {
                UserName = userRegisterDto.UserName,
                Role = userRole,
                Password = BCryptNet.HashPassword(userRegisterDto.Password)
            };

            User? userExists = _unitOfWork.UserRepository.FindByUserName(userRegisterDto.UserName);
            if (userExists != null)
                throw new BadUser("account already exists");

            _unitOfWork.UserRepository.Create(userToCreate);
            _unitOfWork.BaseDirectoryRepository.CreateUserRootDir(userToCreate.UserName);
            bool saved =  await _unitOfWork.UserRepository.SaveAsync();

            if (!saved)
            {
                throw new ErrorDuringSavingContext("user not created; internal error");
            }

            var token = _jwtUtils.GenerateJwtToken(userToCreate);
            return new UserLoginResponseDto(userToCreate, token);
        }
    }
}
