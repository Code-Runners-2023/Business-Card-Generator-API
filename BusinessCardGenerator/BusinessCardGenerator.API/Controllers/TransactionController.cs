using BusinessCardGenerator.API.Data;
using BusinessCardGenerator.API.Models.Transaction;
using BusinessCardGenerator.API.Services.Interfaces;
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

        [HttpGet("transactions")]
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

        [HttpGet("transactions/{transactionId}")]
        public IActionResult GetUserTransactionById(Guid userId, Guid transactionId)
        {
            if (userService.GetById(userId) == null)
                return BadRequest();

            if (!transactionService.CheckIfUserIsOwner(userId, transactionId))
                return NotFound();

            Transaction transaction = transactionService.GetById(transactionId);

            return Ok(new TransactionCompressedInfoModel(transaction));
        }

        [HttpGet("bcards/{bcardId}/transaction")]
        public IActionResult GetBcardTransaction(Guid userId, Guid bcardId)
        {
            if (!bcardService.CheckIfUserIsOwner(userId, bcardId))
                return BadRequest();

            Transaction transaction = transactionService.GetByBcardId(bcardId);

            if (transaction == null)
                return NotFound();

            return Ok(new TransactionCompressedInfoModel(transaction));
        }

        [HttpPost("transactions")]
        public IActionResult CreateNewTransaction(Guid userId, TransactionInputModel newTransaction)
        {
            User user = userService.GetById(userId);
            BusinessCard bcard = bcardService.GetById(newTransaction.BusinessCardId);

            if (user == null || bcard == null)
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

            return NoContent();
        }
    }
}
