using BusinessCardGenerator.API.Data;
using BusinessCardGenerator.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusinessCardGenerator.API.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationContext context;

        public TransactionService(ApplicationContext context)
        {
            this.context = context;
        }

        public List<Transaction> GetAllUserTransactions(Guid userId)
            => context.Transactions
                      .Include(transaction => transaction.BusinessCard)
                      .Where(transaction => transaction.BusinessCard.UserId.Equals(userId))
                      .ToList();

        public Transaction GetById(Guid transactionId)
            => context.Transactions
                      .Include(transaction => transaction.BusinessCard)
                      .FirstOrDefault(transaction => transaction.Id.Equals(transactionId));
        public Transaction GetByBcardId(Guid bcardId)
            => context.Transactions
                      .Include(transaction => transaction.BusinessCard)
                      .FirstOrDefault(transaction => transaction.BusinessCardId.Equals(bcardId));

        public bool CheckIfUserIsOwner(Guid userId, Guid transactionId)
            => context.Transactions
                      .Include(transaction => transaction.BusinessCard)
                      .Any(transaction => transaction.Id.Equals(transactionId) && transaction.BusinessCard.UserId.Equals(userId));

        public bool Add(Transaction transaction)
        {
            if (transaction.Amount < 0)
                return false;

            context.Transactions.Add(transaction);
            context.SaveChanges();

            return true;
        }
    }
}
