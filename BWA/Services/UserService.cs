using System.Reflection.Metadata;
using BWA.Database.Infrastructure;
using BWA.DomainEntities;
using BWA.ServiceEntities;
using BWA.Services.interfaces;
using BWA.Utility;
using BWA.Utility.Exception;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace BWA.Services
{
    public class UserService : IUserService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly ICommonService _commonService;
        public UserService(IUnitOfWork unitOfWork, ICommonService commonService)
        {
            _unitOfWork = unitOfWork;
            _commonService = commonService;
        }

        public async Task<bool> AddUserAsync(AddUserDto requestDto)
        {
            var user = new User
            {
                RoleId = requestDto.RoleId,
                Salutation = requestDto.Salutation,
                FirstName = requestDto.FirstName,
                LastName = requestDto.LastName,
                Email = requestDto.Email,
                ISDCode = requestDto.ISDCode,
                MobileNumber = requestDto.MobileNumber,
                Password = Utils.Encrypt(requestDto.FirstName + "@123"),
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            if (!string.IsNullOrEmpty(requestDto.Image))
            {
                user.Image = Utils.Base64ToBytes(requestDto.Image);
                user.ImageName = requestDto.ImageName;
                user.ImageExtension = requestDto.ImageExtension;
            }

            await _unitOfWork.UserRepository.AddAsync(user);
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> DeleteUserAsync(DeleteUserDto requestDto)
        {
            var user = await _unitOfWork.UserRepository.GetAllAsQueryable()
                .Where(x => x.Id == requestDto.Id
                && !x.IsDeleted)
                .FirstOrDefaultAsync();

            if (user == null)
                throw new BadResultException("User Not Found");

            user.IsDeleted = true;
            user.DeactivatedDate = Utils.CurrentDateTime;
            _unitOfWork.UserRepository.Update(user);
            return await _unitOfWork.SaveChangesAsync();
        }
        public async Task<GetUsersDetailsDto> GetUserByIdAsync(GetUserByIdDto requestDto)
        {
            var user = await _unitOfWork.UserRepository.GetAllAsQueryable()
                .Where(x => x.Id == requestDto.Id
                && !x.IsDeleted)
                .FirstOrDefaultAsync();

            if (user == null)
                throw new BadResultException("User Not Found");

            return new GetUsersDetailsDto()
            {
                Id = user.Id,
                RoleId = user.RoleId,
                //Image = Utils.BytesToBase64(user.Image),
                Salutation = user.Salutation,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                ISDCode = user.ISDCode,
                MobileNumber = user.MobileNumber,
                Image = (user.Image == null || user.Image.Length <= 0) ? "" : Utils.BytesToBase64(user.Image),
                ImageName = user.ImageName != null && user.ImageName.Length > 0 ? user.ImageName : "",
                ImageExtension = user.ImageExtension != null && user.ImageExtension.Length > 0 ? user.ImageExtension : ""
            };
        }
        public async Task<PaginationResult<UserDto>> GetUsersAsync(GetUsersDto requestDto)
        {
            var result = _unitOfWork.UserRepository.GetAllAsQueryable()
               .Include(x => x.Role)
               .Where(x => !x.IsDeleted
               && x.Role.Name.ToLower() != nameof(UserRole.Admin).ToLower())
               .Select(x => new UserDto()
               {
                   Id = x.Id,
                   Salutation = x.Salutation,
                   FirstName = x.FirstName,
                   LastName = x.LastName,
                   Role = x.Role.Name,
                   Image = x.Image != null && x.Image.Length > 0 ? Utils.BytesToBase64(x.Image) : "",
                   ImageName = x.ImageName != null && x.ImageName.Length > 0 ? x.ImageName : "",
                   ImageExtension = x.ImageExtension != null && x.ImageExtension.Length > 0 ? x.ImageExtension : "",
                   CreatedDate = x.CreatedDate,
                   IsActive = x.IsActive
               });

            if (!string.IsNullOrEmpty(requestDto.GlobalSearch))
                result = result.Where(x => x.Salutation.Contains(requestDto.GlobalSearch)
                || x.FirstName.Contains(requestDto.GlobalSearch)
                || x.LastName.Contains(requestDto.GlobalSearch));

            return await _commonService.Pagination(result, requestDto.PageIndex, requestDto.PageSize);
        }
        public async Task<bool> UpdateUserAsync(UpdateUserDto requestDto)
        {
            var user = await _unitOfWork.UserRepository.GetAllAsQueryable()
                .Where(x => x.Id == requestDto.Id
                && !x.IsDeleted)
                .FirstOrDefaultAsync();

            if (user == null)
                throw new KeyNotFoundException("User Not Found");

            user.RoleId = requestDto.RoleId;
            user.Salutation = requestDto.Salutation;
            user.FirstName = requestDto.FirstName;
            user.LastName = requestDto.LastName;
            user.Email = requestDto.Email;
            user.ISDCode = requestDto.ISDCode;
            user.MobileNumber = requestDto.MobileNumber;
            user.ModifiedDate = DateTime.UtcNow;


            if (!string.IsNullOrEmpty(requestDto.Image))
            {
                user.Image = Utils.Base64ToBytes(requestDto.Image);
                user.ImageName = requestDto.ImageName;
                user.ImageExtension = requestDto.ImageExtension;
            }

            if (requestDto.RemoveImage)
            {
                user.ImageName = null;
                user.ImageExtension = null;
                user.Image = new byte[0];
            }

            _unitOfWork.UserRepository.Update(user);
            return await _unitOfWork.SaveChangesAsync();
        }
        public async Task<List<GetISDCodesDto>> GetISDCodesAsync()
        {
            return await _unitOfWork.CategoryRepository.GetAllAsQueryable()
                .Select(x => new GetISDCodesDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToListAsync();
        }

        public async Task<List<RoleDto>> GetRolesAsync()
        {
            return await _unitOfWork.RoleRepository.GetAllAsQueryable()
                .Select(x => new RoleDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToListAsync();
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordDto requestDto)
        {
            var user = await _unitOfWork.UserRepository.GetAllAsQueryable()
                .Where(x => x.IsActive
                && x.Id == requestDto.ActionBy
                && x.Password == Utils.Encrypt(requestDto.OldPassword))
                .FirstOrDefaultAsync();

            user.Password = Utils.Encrypt(requestDto.ConfirmPassword);
            _unitOfWork.UserRepository.Update(user);
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> DeactivateUserAsync(DeactivateUserDto requestDto)
        {
            var user = await _unitOfWork.UserRepository.GetAllAsQueryable()
                .Where(x => x.Id == requestDto.UserId)
                .FirstOrDefaultAsync();

            if (user == null)
                throw new BadResultException("User not found.");

            if (!user.IsActive)
                throw new BadResultException("User is already deactive.");

            user.IsActive = false;
            _unitOfWork.UserRepository.Update(user);
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ActivateUserAsync(ActivateUserDto requestDto)
        {
            var user = await _unitOfWork.UserRepository.GetAllAsQueryable()
                .Where(x => x.Id == requestDto.UserId)
                .FirstOrDefaultAsync();

            if (user == null)
                throw new BadResultException("User not found.");

            if (user.IsActive)
                throw new BadResultException("User is already active.");

            user.IsActive = false;
            _unitOfWork.UserRepository.Update(user);
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<Country>> GetCountryAsync()
        {
            return await _unitOfWork.CountryRepository.GetAllAsQueryable()
                .Select(x => new Country()
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsdCode = x.IsdCode
                }).ToListAsync();
        }

        public async Task<LoginDetailsDto> GetLoginDetailsDto(GetLoginDetailsDto requestDto)
        {
            return await _unitOfWork.UserRepository.GetAllAsQueryable()
                .Include(x => x.Role)
                .Where(x => x.Id == requestDto.ActionBy
                && x.IsActive)
                .Select(x => new LoginDetailsDto()
                {
                    Id = x.Id,
                    Salutation = x.Salutation,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Image = x.Image != null ? Utils.BytesToBase64(x.Image!) : "",
                    ImageExtension = x.ImageExtension,
                    ImageName = x.ImageName,
                    RoleName = x.Role != null ? x.Role!.Name : ""
                })
                .FirstOrDefaultAsync() ?? new LoginDetailsDto();
        }

        public async Task<bool> SignInAsync(SignInDto requestDto)
        {
            var user = new User
            {
                RoleId = (int)UserRole.User,
                Salutation = requestDto.Salutation,
                FirstName = requestDto.FirstName,
                LastName = requestDto.LastName,
                Email = requestDto.Email,
                ISDCode = requestDto.ISDCode,
                MobileNumber = requestDto.MobileNumber,
                Password = Utils.Encrypt(requestDto.FirstName + "@123"),
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            if (!string.IsNullOrEmpty(requestDto.Image))
            {
                user.Image = Utils.Base64ToBytes(requestDto.Image);
                user.ImageName = requestDto.ImageName;
                user.ImageExtension = requestDto.ImageExtension;
            }

            await _unitOfWork.UserRepository.AddAsync(user);
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> UpdateProfileAsync(SignInDto requestDto)
        {
            var user = await _unitOfWork.UserRepository.GetAllAsQueryable()
                .Where(x => x.Id == requestDto.Id)
                .FirstOrDefaultAsync();

            if (user == null)
                throw new BadResultException("User Not Found.");

            user.RoleId = (int)UserRole.User;
            user.Salutation = requestDto.Salutation;
            user.FirstName = requestDto.FirstName;
            user.LastName = requestDto.LastName;
            user.Email = requestDto.Email;
            user.ISDCode = requestDto.ISDCode;
            user.MobileNumber = requestDto.MobileNumber;
            user.ModifiedDate = DateTime.UtcNow;
            user.IsActive = true;

            if (!string.IsNullOrEmpty(requestDto.Image))
            {
                user.Image = Utils.Base64ToBytes(requestDto.Image);
                user.ImageName = requestDto.ImageName;
                user.ImageExtension = requestDto.ImageExtension;
            }

            await _unitOfWork.UserRepository.AddAsync(user);
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
