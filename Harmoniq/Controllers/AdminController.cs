using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Harmoniq.Dtos.UserDtos;
using Harmoniq.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Harmoniq.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")] // ✅ Only admins can access
    public class AdminController : ControllerBase
    {
        private readonly AdminService _adminService;

        public AdminController(AdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("admins")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllAdmins()
        {
            return Ok(await _adminService.GetAllAdminsAsync());
        }

        [HttpPost("promote/{userId}")]
        public async Task<IActionResult> PromoteToAdmin(int userId)
        {
            await _adminService.PromoteToAdminAsync(userId);
            return Ok(new { message = "User promoted to admin." });
        }

        [HttpPost("demote/{userId}")]
        public async Task<IActionResult> DemoteToUser(int userId)
        {
            await _adminService.DemoteToUserAsync(userId);
            return Ok(new { message = "Admin demoted to regular user." });
        }
    }
}
