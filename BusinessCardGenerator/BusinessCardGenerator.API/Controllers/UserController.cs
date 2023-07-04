using BusinessCardGenerator.API.Models;
using BusinessCardGenerator.API.Services;
using BusinessCardGenerator.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BusinessCardGenerator.API.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            List<User> users = UsersDataStore.Current.Users;

            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            List<User> users = UsersDataStore.Current.Users;

            if (id < 0 || id >= users.Count)
                return NotFound();

            User result = users.FirstOrDefault(u => u.Id == id);

            return Ok(result);
        }
    }
}
