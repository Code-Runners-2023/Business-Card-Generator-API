using BusinessCardGenerator.API.Data;
using BusinessCardGenerator.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusinessCardGenerator.API.Controllers
{
    [ApiController]
    [Route("users/{userId}/images")]
    public class ImageController : ControllerBase
    {
        private readonly IImageService imageService;
        private readonly IUserService userService;

        public ImageController(IImageService imageService, IUserService userService)
        {
            this.imageService = imageService;
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult GetAllUserImages(Guid userId)
        {
            if (userService.GetById(userId) == null)
                return BadRequest();
            
            List<string> files = imageService.GetAll(userId)
                                             .Select(image => imageService.GetFromCloud(image.Id))
                                             .ToList();

            return Ok(files);
        }

        [HttpGet("{imageId}")]
        public IActionResult GetUserImageById(Guid userId, Guid imageId)
        {
            if (userService.GetById(userId) == null)
                return BadRequest();

            Image image = imageService.GetById(userId, imageId);

            if (image == null)
                return NotFound();

            string file = imageService.GetFromCloud(image.Id);

            return Ok(file);
        }

        [HttpPost]
        public IActionResult AddNewUserImage(Guid userId, IFormFile file)
        {
            User user = userService.GetById(userId);
            
            if (user == null)
                return BadRequest();

            // save file in cloud storage
            Guid imageId = Guid.NewGuid();
            imageService.SaveInCloud(imageId, file);

            Image image = new Image()
            {
                UserId = userId,
                User = user,
                Path = file.FileName
            };

            imageService.Add(image);

            return NoContent();
        }

        [HttpDelete("{imageId}")]
        public IActionResult RemoveUserImageById(Guid userId, Guid imageId)
        {
            if (userService.GetById(userId) == null)
                return BadRequest();

            Image removed = imageService.Remove(userId, imageId);

            if (removed == null)
                return BadRequest();

            string file = imageService.DeleteFromCloud(imageId);

            return Ok($"{removed.Path} -> {file}");
        }
    }
}
