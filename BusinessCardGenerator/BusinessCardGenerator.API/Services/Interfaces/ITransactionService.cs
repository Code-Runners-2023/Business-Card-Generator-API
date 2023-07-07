using BusinessCardGenerator.API.Data;

namespace BusinessCardGenerator.API.Services.Interfaces
{
    public interface ITransactionService
    {
        List<Transaction> GetAllUserTransactions(Guid userId);

        Transaction GetById(Guid transactionId);

        Transaction GetByBcardId(Guid bcardId);

        bool CheckIfUserIsOwner(Guid userId, Guid transactionId);

        bool Add(Transaction transaction);
    }
}
