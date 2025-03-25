using Microsoft.AspNetCore.Mvc;
using Application.Services;
using Application.DTO;
using Infrastructure.Extensions;
using Microsoft.Extensions.Options;
using Infrastructure.Configurations;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EShopWebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController(UserServices userServices, JwtHelper jwtHelper, IOptions<AuthTokenOptions> authTokenOptions) : ControllerBase
    {
        private readonly UserServices _userServices = userServices;

        [HttpGet]
        public async Task<IActionResult> GetAccountDetails(Guid? userId)
        {
            if (userId == null)
            {
                userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }

            var result = await _userServices.GetProfileDetails(userId);

            if (result == null)
            {
                return new JsonResult(NotFound());
            }

            return new JsonResult(Ok(result));
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var user = await _userServices.FindByIdAsync(userId);
            if (user == null)
            {
                return new JsonResult(NotFound());
            }
            return new JsonResult(Ok(user));
        }

        [HttpGet]
        public async Task<IActionResult> GetUserByEmailAsync(string email)
        {
            var result = await _userServices.GetUserByEmail(email);
            if (result == null)
            {
                return new JsonResult(NotFound());
            }
            return new JsonResult(Ok(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetUserByUsernameAsync(string username)
        {
            var result = await _userServices.GetUserByUsername(username);
            if (result == null)
            {
                return new JsonResult(NotFound());
            }
            return new JsonResult(Ok(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetPagedUsersAsync(int page, int size)
        {
            var result = await _userServices.GetPagedUsersAsync(page, size);
            return new JsonResult(Ok(result));
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDTO updatePasswordDTO)
        {
            try
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (currentUserId == updatePasswordDTO.UserId.ToString())
                    return new JsonResult(await _userServices.UpdatePasswordAsync(updatePasswordDTO));
            }
            catch (KeyNotFoundException ex)
            {
                return new JsonResult(NotFound($"{ex.GetType().Name}: {ex.Message}"));
            }
            return new JsonResult(BadRequest());
        }
    }
}
