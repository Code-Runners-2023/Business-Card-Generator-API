using BusinessCardGenerator.API.Data;
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
            List<User> users = userService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            User result = userService.GetById(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public IActionResult AddNewUser(User user)
        {
            if (!ModelState.IsValid || !userService.Add(user))
                return BadRequest();

            return NoContent();
        }

        [HttpPut]
        public IActionResult UpdateUser(User user)
        {
            if (!ModelState.IsValid || !userService.Update(user))
                return BadRequest();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult RemoveUserById(int id)
        {
            User removed = userService.Remove(id);

            if (removed == null)
                return BadRequest();

            return Ok(removed);
        }
    }
}
