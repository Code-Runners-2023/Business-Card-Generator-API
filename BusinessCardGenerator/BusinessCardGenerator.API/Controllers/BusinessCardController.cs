using BusinessCardGenerator.API.Data;
using BusinessCardGenerator.API.Models.BusinessCard;
using BusinessCardGenerator.API.Models.User;
using BusinessCardGenerator.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BusinessCardGenerator.API.Models.Image;
using BusinessCardGenerator.API.Services;
using static System.Net.Mime.MediaTypeNames;

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
        public IActionResult GetAllUserBcards(Guid userId)
        {
            if (userService.GetById(userId) == null)
                return BadRequest();

            List<BusinessCardCompressedInfoModel> compressedBcards = bcardService.GetAll(userId)
                                                                       .Select(bcard => 
                                                                               new BusinessCardCompressedInfoModel(bcard,
                                                                               bcardService.GetLogoFromCloud(bcard.Id)))
                                                                       .ToList();

            return Ok(compressedBcards);
        }

        [HttpGet("{bcardId}")]
        public IActionResult GetUserBcardById(Guid userId, Guid bcardId)
        {
            if (userService.GetById(userId) == null)
                return BadRequest();

            if (!bcardService.CheckIfUserIsOwner(userId, bcardId))
                return NotFound();

            BusinessCard bcard = bcardService.GetById(bcardId);

            string logoFile = bcardService.GetLogoFromCloud(bcard.Id);

            return Ok(new BusinessCardCompressedInfoModel(bcard, logoFile));
        }

        [HttpPost("upload")]
        public IActionResult UploadNewUserBcard(Guid userId, BusinessCardInputModel userInput)
        {
            User user = userService.GetById(userId);

            if (user == null || !ModelState.IsValid)
                return BadRequest();

            Guid bcardId = Guid.NewGuid();
            IFormFile logoFile = Request.Form.Files[0];
            bcardService.SaveLogoInCloud(bcardId, logoFile);

            BusinessCard bcard = new BusinessCard()
            {
                Id = bcardId,
                UserId = userId,
                User = user,
                Name = userInput.Name,
                Address = userInput.Address,
                Website = userInput.Website,
                LogoPath = logoFile.FileName,
                RGBColorCode = userInput.RGBColorCode
            };

            bcardService.Add(bcard);

            return NoContent();
        }

        [HttpPatch("{bcardId}")]
        public IActionResult UpdateUserBcard(Guid userId, Guid bcardId, BusinessCardInputModel model)
        {
            if (!ModelState.IsValid || userService.GetById(userId) == null)
                return BadRequest();

            BusinessCard bcard = bcardService.GetById(bcardId);

            if (bcard == null)
                return NotFound();

            // update logo in cloud

            string logoPath = bcardService.GetLogoPathInCloud(bcardId);

            bcard.ApplyChanges(model, logoPath);

            bcardService.Update(bcard);

            return NoContent();
        }

        [HttpDelete("{bcardId}")]
        public IActionResult RemoveUserBcardById(Guid userId, Guid bcardId)
        {
            if (userService.GetById(userId) == null)
                return BadRequest();

            BusinessCard removed = bcardService.Remove(userId, bcardId);

            if (removed == null)
                return BadRequest();

            string logoFile = bcardService.DeleteLogoFromCloud(bcardId);

            return Ok(new BusinessCardCompressedInfoModel(removed, logoFile));
        }
    }
}
