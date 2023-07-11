using BusinessCardGenerator.API.Data;
using BusinessCardGenerator.API.Models.BusinessCard;
using BusinessCardGenerator.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusinessCardGenerator.API.Controllers
{
    [ApiController]
    [Route("users/{userId}/bcards")]
    public class BusinessCardController : ControllerBase
    {
        private readonly IBusinessCardService bcardService;
        private readonly IUserService userService;
        private readonly IAzureCloudService azureCloudService;

        private readonly string[] allowedFileContentTypes = { "image/jpeg", "image/png" };

        public BusinessCardController(IBusinessCardService bcardService, IUserService userService,
                                      IAzureCloudService azureCloudService)
        {
            this.bcardService = bcardService;
            this.userService = userService;
            this.azureCloudService = azureCloudService;
        }

        [HttpGet, Authorize]
        public IActionResult GetAllUserBcards(Guid userId)
        {
            if (userService.GetById(userId) == null)
                return BadRequest();

            List<BusinessCardCompressedInfoModel> compressedBcards = bcardService
                                                                       .GetAll(userId)
                                                                       .Select(bcard => 
                                                                               new BusinessCardCompressedInfoModel(bcard,
                                                                               azureCloudService.GetFileFromCloud(bcard.Id)))
                                                                       .ToList();

            return Ok(compressedBcards);
        }

        [HttpGet("{bcardId}"), Authorize]
        public IActionResult GetUserBcardById(Guid userId, Guid bcardId)
        {
            if (userService.GetById(userId) == null)
                return BadRequest();

            if (!bcardService.CheckIfUserIsOwner(userId, bcardId))
                return NotFound();

            BusinessCard bcard = bcardService.GetById(bcardId);

            byte[] logoFile = azureCloudService.GetFileFromCloud(bcard.Id);

            return Ok(new BusinessCardCompressedInfoModel(bcard, logoFile));
        }

        [HttpPost("upload"), Authorize]
        public IActionResult UploadNewUserBcard(Guid userId, [FromForm] BusinessCardInputModel userInput)
        {
            User user = userService.GetById(userId);

            if (user == null || !ModelState.IsValid || !IsFileContentTypeAllowed(userInput.LogoFile.ContentType))
                return BadRequest();

            Guid bcardId = Guid.NewGuid();

            BusinessCard bcard = new BusinessCard()
            {
                Id = bcardId,
                UserId = userId,
                User = user,
                Name = userInput.Name,
                Address = userInput.Address,
                Website = userInput.Website,
                HexColorCodeMain = userInput.HexColorCodeMain,
                HexColorCodeSecondary = userInput.HexColorCodeSecondary
            };

            bcardService.Add(bcard);
            azureCloudService.SaveFileInCloud(bcardId, userInput.LogoFile);

            return NoContent();
        }

        [HttpPatch("{bcardId}"), Authorize]
        public IActionResult UpdateUserBcard(Guid userId, Guid bcardId, [FromForm] BusinessCardInputModel model)
        {
            if (!ModelState.IsValid || !IsFileContentTypeAllowed(model.LogoFile.ContentType) 
                                    || userService.GetById(userId) == null)
                return BadRequest();

            BusinessCard bcard = bcardService.GetById(bcardId);

            if (bcard == null)
                return NotFound();

            bcard.ApplyChanges(model);

            bcardService.Update(bcard);
            azureCloudService.UpdateFileInCloud(bcardId, model.LogoFile);

            return NoContent();
        }

        [HttpDelete("{bcardId}"), Authorize]
        public IActionResult RemoveUserBcardById(Guid userId, Guid bcardId)
        {
            if (userService.GetById(userId) == null)
                return BadRequest();

            BusinessCard removed = bcardService.Remove(userId, bcardId);

            if (removed == null)
                return BadRequest();

            byte[] logoFile = azureCloudService.DeleteFileFromCloud(bcardId);

            return Ok(new BusinessCardCompressedInfoModel(removed, logoFile));
        }

        private bool IsFileContentTypeAllowed(string contentType)
            => allowedFileContentTypes.Contains(contentType);
    }
}
