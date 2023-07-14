using BusinessCardGenerator.API.Data;
using BusinessCardGenerator.API.Models.Transaction;
using BusinessCardGenerator.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BusinessCardGenerator.API.Controllers
{
    [ApiController]
    [Route("users/{userId}")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService transactionService;
        private readonly IUserService userService;
        private readonly IBusinessCardService bcardService;

        public TransactionController(ITransactionService transactionService, IUserService userService,
                                     IBusinessCardService bcardService)
        {
            this.transactionService = transactionService;
            this.userService = userService;
            this.bcardService = bcardService;
        }

        [HttpGet("transactions"), Authorize]
        public IActionResult GetAllUserBcardTransactions(Guid userId)
        {
            if (userService.GetById(userId) == null)
                return BadRequest();

            List<TransactionCompressedInfoModel> compressedTransactions = transactionService
                                                                          .GetAllUserTransactions(userId)
                                                                          .Select(transaction => 
                                                                                  new TransactionCompressedInfoModel(transaction))
                                                                          .ToList();

            return Ok(compressedTransactions);
        }

        [HttpGet("transactions/{transactionId}"), Authorize]
        public IActionResult GetUserTransactionById(Guid userId, Guid transactionId)
        {
            if (userService.GetById(userId) == null)
                return BadRequest();

            if (!transactionService.CheckIfUserIsOwner(userId, transactionId))
                return NotFound();

            Transaction transaction = transactionService.GetById(transactionId);

            return Ok(new TransactionCompressedInfoModel(transaction));
        }

        [HttpGet("bcards/{bcardId}/transactions"), Authorize]
        public IActionResult GetBcardTransaction(Guid userId, Guid bcardId)
        {
            if (!bcardService.CheckIfUserIsOwner(userId, bcardId))
                return BadRequest();

            List<TransactionCompressedInfoModel> compressedTransactions = transactionService
                                                                          .GetAllByBcardId(bcardId)
                                                                          .Select(transaction => new TransactionCompressedInfoModel(transaction))
                                                                          .ToList();

            return Ok(compressedTransactions);
        }

        [HttpPost("transactions"), Authorize]
        public IActionResult CreateNewTransaction(Guid userId, TransactionInputModel newTransaction)
        {
            User user = userService.GetById(userId);
            BusinessCard bcard = bcardService.GetById(newTransaction.BusinessCardId);

            if (user == null || bcard == null || (user.Balance - Math.Abs(newTransaction.Amount) < 0))
                return BadRequest();

            Transaction transaction = new Transaction()
            {
                Id = Guid.NewGuid(),
                BusinessCardId = bcard.Id,
                BusinessCard = bcard,
                Amount = newTransaction.Amount,
                Date = newTransaction.Date
            };

            if (!transactionService.Add(transaction))
                return BadRequest();

            user.Balance -= newTransaction.Amount;
            userService.Update(user);

            return NoContent();
        }
    }
}
