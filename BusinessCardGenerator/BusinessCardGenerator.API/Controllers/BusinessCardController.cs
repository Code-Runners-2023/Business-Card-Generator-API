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
        private readonly ITransactionService transactionService;
        private readonly IUserService userService;
        private readonly IAzureCloudService azureCloudService;
        private readonly ApplicationSettings settings;

        private readonly string[] allowedFileContentTypes = { "image/jpeg", "image/png" };

        public BusinessCardController(IBusinessCardService bcardService, ITransactionService transactionService,
                                      IUserService userService, IAzureCloudService azureCloudService,
                                      IConfiguration config)
        {
            this.bcardService = bcardService;
            this.transactionService = transactionService;
            this.userService = userService;
            this.azureCloudService = azureCloudService;
            settings = new ApplicationSettings(config);
        }

        [HttpGet, Authorize]
        public IActionResult GetAllUserBcards(Guid userId)
        {
            if (userService.GetById(userId) == null)
                return BadRequest();

            List<BusinessCardCompressedInfoModel> compressedBcards = bcardService
                                                                     .GetAll(userId)
                                                                     .Select(bcard =>
                                                                             new BusinessCardCompressedInfoModel(bcard, settings.AzureBlobUrl))
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

            return Ok(new BusinessCardCompressedInfoModel(bcard, settings.AzureBlobUrl));
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
                LogoFileExtension = Path.GetExtension(userInput.LogoFile.FileName),
                HexColorCodeMain = userInput.HexColorCodeMain,
                HexColorCodeSecondary = userInput.HexColorCodeSecondary
            };

            bcardService.Add(bcard);
            azureCloudService.UploadFileInCloud(bcardId, userInput.LogoFile);

            return NoContent();
        }

        [HttpPatch("{bcardId}"), Authorize]
        public IActionResult UpdateUserBcard(Guid userId, Guid bcardId, [FromForm] BusinessCardInputModel model)
        {
            if (!ModelState.IsValid || !IsFileContentTypeAllowed(model.LogoFile.ContentType) || userService.GetById(userId) == null)
                return BadRequest();

            BusinessCard bcard = bcardService.GetById(bcardId);

            if (bcard == null)
                return NotFound();

            string oldLogoFileExtention = bcard.LogoFileExtension;
            bcard.ApplyChanges(model);

            bcardService.Update(bcard);
            azureCloudService.UpdateFileInCloud(bcardId, oldLogoFileExtention, model.LogoFile);

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

            transactionService.RemoveAllWithBcardId(removed.Id);
            azureCloudService.DeleteFileFromCloud(bcardId, removed.LogoFileExtension);

            return Ok();
        }

        private bool IsFileContentTypeAllowed(string contentType)
            => allowedFileContentTypes.Contains(contentType);
    }
}
