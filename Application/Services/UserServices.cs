using Application.Common.Pagination;
using Application.DTO;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class UserServices(IUnitOfWork unitOfWork, UserManager<User> userManager, IMapper mapper)
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IGenericRepository<User> _userRepository = unitOfWork.GetRepository<User>();
        private readonly IGenericRepository<Order> _orderRepository = unitOfWork.GetRepository<Order>();
        private readonly UserManager<User> _userManager = userManager;
        private readonly IMapper _mapper = mapper;

        public async Task<User?> FindByLoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Login);

            user ??= await _userManager.FindByEmailAsync(dto.Login);

            return user;
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user;
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return user;
        }

        public async Task<IdentityResult> Register(RegisterDto dto)
        {
            var user = new User { UserName = dto.UserName, Email = dto.Email , Address = dto.Address};
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Customer");
            }

            return result;
        }
        public async Task<bool> Login(LoginDto dto)
        {
            var user = await this.FindByLoginAsync(dto);

            if (user == null)
            {
                return false;
            }

            var result = await _userManager.CheckPasswordAsync(user, dto.Password);

            return result;
        }

        public async Task<UserProfileDto?> GetProfileDetails(Guid? id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return null;
            }

            var result = _mapper.Map<UserProfileDto>(user);

            result.Roles = await _userManager.GetRolesAsync(user);

            return result;
        }
        public async Task<User?> FindByIdAsync(Guid userId)
        {
            return await _userManager.FindByIdAsync(userId.ToString());
        }

        public async Task<PagedResult<User>> GetPagedUsersAsync(int page, int size)
        {
            var (items, totalCount) = await _userRepository.GetPagedAsync(page, size);
            return new PagedResult<User>(items, totalCount, page, size);
        }

        public async Task<UpdateInfoDto> UpdateInfo(UpdateInfoDto dto)
        {
            var user = await _userRepository.GetByIdAsync(dto.Id);
            if (user == null)
            {
                return null;
            }
            user.PhoneNumber = dto.PhoneNumber;
            user.Address = dto.Address;
            await _unitOfWork.SaveChangesAsync();
            return dto;
        }

        public async Task<IdentityResult> UpdatePassword(UpdatePasswordDto dto)
        {
            var updateUser = await _userManager.FindByIdAsync(dto.Id.ToString());
            return await _userManager.ChangePasswordAsync(updateUser, dto.OldPassword, dto.NewPassword);
        }

        public async Task<bool> DeleteUser(Guid userId)
        {
            if (await _userRepository.GetByIdAsync(userId) == null) return false;
            await _userRepository.DeleteAsync(userId);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResult<Order>> GetPagedOrderHistory(int page, int size, string userId)
        {
            if (!Guid.TryParse(userId, out Guid id))
            {
                throw new ArgumentException("Invalid userId format");
            }

            var (items, totalCount) = await _orderRepository.GetPagedAsync(
                page,
                size,
                x => x.UserId == id,
                y => y.OrderByDescending(x => x.OrderDate)
            );

            return new PagedResult<Order>(items, totalCount, page, size);
        }

        public async Task<UserStatusDto> ChangeUserStatus(UserStatusDto userStatusDto)
        {
            var user = await _userRepository.GetByIdAsync(userStatusDto);
            if (userStatusDto.ban)
            {         
                user.Active = true;
            }
            else
            {
                user.Active = false;    
            }
            await _unitOfWork.SaveChangesAsync();
            return userStatusDto;
        }

    }
}
