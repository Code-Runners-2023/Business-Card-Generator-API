using BusinessCardGenerator.API.Data;
using BusinessCardGenerator.API.Models.Image;
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
        private readonly IBusinessCardService bcardService;
        private readonly ITransactionService transactionService;
        private readonly IImageService imageService;
        private readonly IDepositService depositService;
        private readonly IAzureCloudService azureCloudService;

        public UserController(IUserService userService, ITokenService tokenService,
                              IBusinessCardService bcardService, ITransactionService transactionService,
                              IImageService imageService, IDepositService depositService, IAzureCloudService azureCloudService)
        {
            this.userService = userService;
            this.tokenService = tokenService;
            this.bcardService = bcardService;
            this.transactionService = transactionService;
            this.imageService = imageService;
            this.depositService = depositService;
            this.azureCloudService = azureCloudService;
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
            if (userService.GetById(id) == null)
                return BadRequest();

            List<ImageAzureFileModel> azureBcardFiles = bcardService.RemoveAll(id);
            azureBcardFiles.ForEach(file => azureCloudService.DeleteFileFromCloud(file.Id, file.FileExtension));

            List<ImageAzureFileModel> azureImageFiles = imageService.RemoveAll(id);
            azureImageFiles.ForEach(file => azureCloudService.DeleteFileFromCloud(file.Id, file.FileExtension));
            
            depositService.RemoveAllUserDeposits(id);
            transactionService.RemoveAllUserTransactions(id);

            User removed = userService.Remove(id);

            return Ok(new UserCompressedInfoModel(removed));
        }
    }
}
