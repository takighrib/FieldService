using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using UsersApi.Dtos.Admin;

namespace UsersApi.Controller
{
    [Route("api/admin-dashboard")]
    [ApiController]
    public class DashbordController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public DashbordController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("users-names")]
        public async Task<IActionResult> GetAllTechnicians()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                var technicianDtos = new List<DashUserDto>();

                foreach (var user in users)
                {
                    Console.WriteLine("Hello, World!");
                    var roles = await _userManager.GetRolesAsync(user);
                    var role = roles.FirstOrDefault();

                    if (role == "User") // Filter only technicians
                    {
                        technicianDtos.Add(new DashUserDto
                        {
                            UserName = user.UserName,
                            Role = role,
                        });
                    }
                }
            
                return Ok(technicianDtos);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }


    }
}
