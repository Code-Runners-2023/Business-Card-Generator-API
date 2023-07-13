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
        private readonly IAzureCloudService azureCloudService;
        private readonly ApplicationSettings settings;

        private readonly string[] allowedFileContentTypes = { "image/jpeg", "image/png" };

        public ImageController(IImageService imageService, IUserService userService,
                               IAzureCloudService azureCloudService, IConfiguration config)
        {
            this.imageService = imageService;
            this.userService = userService;
            this.azureCloudService = azureCloudService;
            settings = new ApplicationSettings(config);
        }

        [HttpGet, Authorize]
        public IActionResult GetAllUserImages(Guid userId)
        {
            if (userService.GetById(userId) == null)
                return BadRequest();
            
            List<ImageCompressedInfoModel> compressedImages = imageService
                                                              .GetAll(userId)
                                                              .Select(image => new ImageCompressedInfoModel(image, settings.AzureBlobUrl))
                                                              .ToList();

            return Ok(compressedImages);
        }

        [HttpGet("{imageId}"), Authorize]
        public IActionResult GetUserImageById(Guid userId, Guid imageId)
        {
            if (userService.GetById(userId) == null)
                return BadRequest();

            if (!imageService.CheckIfUserIsOwner(userId, imageId))
                return NotFound();

            Image image = imageService.GetById(imageId);

            return Ok(new ImageCompressedInfoModel(image, settings.AzureBlobUrl));
        }

        [HttpPost("upload"), Authorize]
        public IActionResult UploadNewUserImage(Guid userId, IFormFile file)
        {
            User user = userService.GetById(userId);
            
            if (user == null || file == null || file.Length == 0 || !IsFileContentTypeAllowed(file.ContentType))
                return BadRequest();

            Guid imageId = Guid.NewGuid();

            Image image = new Image()
            {
                Id = imageId,
                UserId = userId,
                User = user,
                FileExtension = Path.GetExtension(file.FileName)
            };

            azureCloudService.UploadFileInCloud(imageId, file);
            imageService.Add(image);

            return NoContent();
        }

        [HttpDelete("{imageId}"), Authorize]
        public IActionResult RemoveUserImageById(Guid userId, Guid imageId)
        {
            if (userService.GetById(userId) == null)
                return BadRequest();

            Image image = imageService.Remove(userId, imageId);

            if (image == null)
                return BadRequest();

            azureCloudService.DeleteFileFromCloud(imageId, image.FileExtension);

            return Ok();
        }

        private bool IsFileContentTypeAllowed(string contentType)
            => allowedFileContentTypes.Contains(contentType);
    }
}
