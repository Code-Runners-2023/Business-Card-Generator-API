using BusinessCardGenerator.API.Data;
using BusinessCardGenerator.API.Models.Image;
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
            
            List<ImageCompressedInfoModel> compressedImages = imageService
                                                              .GetAll(userId)
                                                              .Select(image => new ImageCompressedInfoModel(image.Id,
                                                                                   imageService.GetFromCloud(image.Id)))
                                                              .ToList();

            return Ok(compressedImages);
        }

        [HttpGet("{imageId}")]
        public IActionResult GetUserImageById(Guid userId, Guid imageId)
        {
            if (userService.GetById(userId) == null)
                return BadRequest();

            if (!imageService.CheckIfUserIsOwner(userId, imageId))
                return NotFound();

            string file = imageService.GetFromCloud(imageId);

            return Ok(new ImageCompressedInfoModel(imageId, file));
        }

        [HttpPost("upload")]
        public IActionResult UploadNewUserImage(Guid userId, IFormFile file)
        {
            User user = userService.GetById(userId);
            
            if (user == null)
                return BadRequest();

            // save file in cloud storage
            Guid imageId = Guid.NewGuid();
            imageService.SaveInCloud(imageId, file);

            Image image = new Image()
            {
                Id = imageId,
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
            if (userService.GetById(userId) == null || imageService.Remove(userId, imageId) == null)
                return BadRequest();

            string file = imageService.DeleteFromCloud(imageId);

            return Ok(new ImageCompressedInfoModel(imageId, file));
        }
    }
}
