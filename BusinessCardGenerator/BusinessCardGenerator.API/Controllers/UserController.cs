using BusinessCardGenerator.API.Data;
using BusinessCardGenerator.API.Models.User;
using BusinessCardGenerator.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusinessCardGenerator.API.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ITokenService tokenService;

        public UserController(IUserService userService, ITokenService tokenService)
        {
            this.userService = userService;
            this.tokenService = tokenService;
        }

        [HttpGet, Authorize]
        public IActionResult GetAllUsers()
        {
            List<UserCompressedInfoModel> users = userService.GetAll()
                                                             .Select(user => new UserCompressedInfoModel(user))
                                                             .ToList();

            return Ok(users);
        }

        [HttpGet("{id}"), Authorize]
        public IActionResult GetUserById(Guid id)
        {
            User result = userService.GetById(id);

            if (result == null)
                return NotFound();

            return Ok(new UserCompressedInfoModel(result));
        }

        [HttpPost("register")]
        public IActionResult RegisterNewUser(UserInputModel userInput)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (userService.IsUserRegisteredWithEmail(userInput.Email))
                return Conflict();

            User user = new User(userInput);

            userService.Add(user);

            string jwtToken = tokenService.GenerateNewToken(user);

            return Ok(new UserAuthResponse(user, jwtToken));
        }

        [HttpPost("login")]
        public IActionResult ValidateLoginAttempt(UserLoginModel login)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (!userService.VerifyLogin(login))
                return Unauthorized();

            User user = userService.GetByEmailAndPassword(login.Email, login.Password);
            string jwtToken = tokenService.GenerateNewToken(user);

            return Ok(new UserAuthResponse(user, jwtToken));
        }

        [HttpPatch("{id}"), Authorize]
        public IActionResult UpdateUser(Guid id, UserInputModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            User user = userService.GetById(id);

            if (user == null)
                return NotFound();

            if (user.Email != model.Email && userService.IsUserRegisteredWithEmail(model.Email))
                return Conflict();

            user.ApplyChanges(model);

            userService.Update(user);

            return NoContent();
        }

        [HttpDelete("{id}"), Authorize]
        public IActionResult RemoveUserById(Guid id)
        {
            User removed = userService.Remove(id);

            if (removed == null)
                return BadRequest();

            return Ok(new UserCompressedInfoModel(removed));
        }
    }
}
