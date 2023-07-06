using BusinessCardGenerator.API.Data;
using BusinessCardGenerator.API.Models.User;
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
            List<UserCompressedInfoModel> users = userService.GetAll()
                                                             .Select(user => new UserCompressedInfoModel(user))
                                                             .ToList();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(Guid id)
        {
            User result = userService.GetById(id);

            if (result == null)
                return NotFound();

            return Ok(new UserCompressedInfoModel(result));
        }

        [HttpPost]
        public IActionResult AddNewUser(UserInputModel userInput)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            userService.Add(new User(userInput));

            return NoContent();
        }

        [HttpPost("login")]
        public IActionResult ValidateLoginAttempt(UserLoginModel login)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (!userService.VerifyLogin(login))
                return NotFound();

            User user = userService.GetByEmailAndPassword(login.Email, login.Password);

            return Ok(new UserCompressedInfoModel(user));
        }

        [HttpPatch]
        public IActionResult UpdateUser(UserInputModel userInput)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            userService.Update(new User(userInput));

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult RemoveUserById(Guid id)
        {
            User removed = userService.Remove(id);

            if (removed == null)
                return BadRequest();

            return Ok(new UserCompressedInfoModel(removed));
        }
    }
}
