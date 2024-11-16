using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoList.API.Models;
using ToDoList.API.Repositories;
using ToDoList.API.Services;

namespace ToDoList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepository<User> _userRepository;
        private readonly ITokenService _tokenService;

        public UserController(IRepository<User> userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User login)
        {
            var user = (await _userRepository.FindAsync(u => u.Username == login.Username && u.Password == login.Password))
                        .FirstOrDefault();

            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            var token = _tokenService.GenerateToken(user);
            return Ok(new { Token = token });
        }
    }
}
