using BWA.Database.Infrastructure;
using BWA.DomainEntities;
using BWA.ServiceEntities;
using BWA.Services.interfaces;
using BWA.Utility;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace BWA.Services
{
    public class AuthService : IAuthService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IJwtService _jwtService;

        public AuthService(IUnitOfWork unitOfWork
            , IJwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
        }

        public async Task<LoginDetails> LoginAsync(LoginDto requestDto)
        {
            var userExist = await _unitOfWork.UserRepository.GetAllAsQueryable()
                .Where(x => (x.Email == requestDto.UserName || x.MobileNumber == requestDto.UserName)
                && x.Password == Utils.Encrypt(requestDto.Password)
                && x.IsActive)
                .FirstOrDefaultAsync();

            if (userExist == null)
                throw new KeyNotFoundException("Incorrect username or password");

            return await GenerateJwtTokenAsync(new JWTTokenGenerationDto()
            {
                RoleId = userExist.RoleId,
                UserId = userExist.Id,
            });
        }

        public async Task<bool> LogOutAsync(LogOutDto requestDto)
        {
            var userExist = await _unitOfWork.ConnectionRepository.GetAllAsQueryable()
               .Where(x => x.UserId == requestDto.ActionBy
               && !x.IsLogout)
               .FirstOrDefaultAsync();

            if (userExist == null)
                throw new KeyNotFoundException("User not found.");

            await _unitOfWork.ConnectionHistoryRepository.AddAsync(new ConnectionHistory()
            {
                CreatedDate = Utils.CurrentDateTime,
                ExpiryDateTime = userExist.ExpiryDateTime,
                IsLogout = true,
                JWTToken = userExist.JWTToken,
                UserId = requestDto.ActionBy
            });

            _unitOfWork.ConnectionRepository.Delete(userExist);

            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<LoginDetails> GenerateJwtTokenAsync(JWTTokenGenerationDto requestDtoDto)
        {
            var role = await _unitOfWork.RoleRepository.GetAllAsQueryable()
                .Where(x => x.Id == requestDtoDto.RoleId)
                .FirstOrDefaultAsync() ?? new Role();

            var token = _jwtService.GenerateSecurityToken(new SessionDetailsDto()
            {
                RoleId = requestDtoDto.RoleId,
                UserId = requestDtoDto.UserId,
                Role = role.Name
            }, out DateTime ExpirationTime);

            await _unitOfWork.ConnectionRepository.AddAsync(new Connection()
            {
                CreatedDate = Utils.CurrentDateTime,
                ExpiryDateTime = ExpirationTime,
                IsLogout = false,
                JWTToken = token,
                UserId = requestDtoDto.UserId
            });

            await _unitOfWork.ConnectionHistoryRepository.AddAsync(new ConnectionHistory()
            {
                CreatedDate = Utils.CurrentDateTime,
                ExpiryDateTime = ExpirationTime,
                IsLogout = false,
                JWTToken = token,
                UserId = requestDtoDto.UserId
            });

            await _unitOfWork.SaveChangesAsync();

            return new LoginDetails()
            {
                //UserId = "",
                UserId = Utils.Encrypt(Convert.ToString(requestDtoDto.UserId)),
                RoleName = role.Name,
                Token = token
            };

        }

        public async Task<LoginDetails> MaintainLoginConnectionHistoryAsync(MaintainLoginConnectionHistoryDto requestDtoDto )
        {
            //var role = await _unitOfWork.Curre.GetAllAsQueryable()
            //    .Where(x => x.Id == requestDtoDto.RoleId)
            //    .FirstOrDefaultAsync() ?? new Role();

            //var token = _jwtService.GenerateSecurityToken(new SessionDetailsDto()
            //{
            //    RoleId = requestDtoDto.RoleId,
            //    UserId = requestDtoDto.UserId
            //}, out DateTime ExpirationTime);

            //return new LoginDetails()
            //{
            //    UserId = Utils.Encrypt(Convert.ToString(requestDtoDto.UserId)),
            //    Role = role.Name,
            //    Token = token
            //};

            return new LoginDetails();

        }
    }
}
