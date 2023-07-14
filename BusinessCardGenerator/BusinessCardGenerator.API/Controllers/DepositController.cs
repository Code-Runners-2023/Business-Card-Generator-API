using BusinessCardGenerator.API.Data;
using BusinessCardGenerator.API.Models.BusinessCard;
using BusinessCardGenerator.API.Models.Deposit;
using BusinessCardGenerator.API.Services;
using BusinessCardGenerator.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace BusinessCardGenerator.API.Controllers
{
    [ApiController]
    [Route("users/{userId}/deposits")]
    public class DepositController : ControllerBase
    {
        private readonly IDepositService depositService;
        private readonly IUserService userService;

        public DepositController(IDepositService depositService, IUserService userService)
        {
            this.depositService = depositService;
            this.userService = userService;
        }

        [HttpGet, Authorize]
        public IActionResult GetAllUserDeposits(Guid userId)
        {
            if (userService.GetById(userId) == null)
                return BadRequest();

            List<DepositCompressedInfoModel> compressedBcards = depositService
                                                                .GetAllUserDeposits(userId)
                                                                .Select(deposit =>
                                                                        new DepositCompressedInfoModel(deposit))
                                                                .ToList();

            return Ok(compressedBcards);
        }

        [HttpGet("{depositId}"), Authorize]
        public IActionResult GetDepositById(Guid userId, Guid depositId)
        {
            if (userService.GetById(userId) == null)
                return BadRequest();

            if (!depositService.CheckIfUserIsOwner(userId, depositId))
                return NotFound();

            Deposit deposit = depositService.GetById(depositId);

            return Ok(new DepositCompressedInfoModel(deposit));
        }

        [HttpPost, Authorize]
        public IActionResult CreateNewDeposit(Guid userId, DepositInputModel newDeposit)
        {
            User user = userService.GetById(userId);

            if (user == null)
                return BadRequest();

            Deposit deposit = new Deposit()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                User = user,
                Amount = newDeposit.Amount,
                Date = newDeposit.Date,
                Status = newDeposit.Status,
                StripeId = newDeposit.StripeId
            };

            if (!depositService.Add(deposit))
                return BadRequest();

            user.Balance += newDeposit.Amount;
            userService.Update(user);

            return NoContent();
        }
    }
}
