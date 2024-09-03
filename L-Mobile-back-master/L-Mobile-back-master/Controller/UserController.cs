using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsersApi.Dtos;
using UsersApi.Dtos.Admin;
using UsersApi.Mapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace api.Controller
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserController(ApplicationDbContext applicationDBContext, UserManager<User> userManager)
        {
            _context = applicationDBContext;
            _userManager = userManager;
        }

        [HttpGet("test")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetUserCount()
        {
            var userCount = await _userManager.Users.CountAsync();
            return Ok(userCount);
        }
    }
}
