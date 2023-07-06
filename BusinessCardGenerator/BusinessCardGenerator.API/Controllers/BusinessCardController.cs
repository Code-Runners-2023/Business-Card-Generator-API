using BusinessCardGenerator.API.Models.User;
using BusinessCardGenerator.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BusinessCardGenerator.API.Controllers
{
    [ApiController]
    [Route("users/{userId}/bcards")]
    public class BusinessCardController : ControllerBase
    {
        private readonly IBusinessCardService bcardService;
        private readonly IUserService userService;

        public BusinessCardController(IBusinessCardService bcardService, IUserService userService)
        {
            this.bcardService = bcardService;
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult GetAllUserBcards(Guid userId) { }

        [HttpGet("{bcardId}")]
        public IActionResult GetUserBcardById(Guid userId, Guid bcardId) { }

        [HttpPost("upload")]
        public IActionResult UploadNewUserBcard(Guid userId, UserInputModel userInput) { }

        [HttpPatch("{bcardId}")]
        public IActionResult UpdateUserBcard(Guid userId, Guid bcardId, UserInputModel model) { }

        [HttpDelete("{imageId}")]
        public IActionResult RemoveUserImageById(Guid userId, Guid imageId) { }
    }
}
