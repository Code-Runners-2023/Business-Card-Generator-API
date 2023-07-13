using BusinessCardGenerator.API.Data;

namespace BusinessCardGenerator.API.Services.Interfaces
{
    public interface ITransactionService
    {
        List<Transaction> GetAllUserTransactions(Guid userId);

        List<Transaction> GetAllByBcardId(Guid bcardId);

        Transaction GetById(Guid transactionId);

        Transaction GetByBcardId(Guid bcardId);

        bool CheckIfUserIsOwner(Guid userId, Guid transactionId);

        bool Add(Transaction transaction);

        void RemoveAllWithBcardId(Guid bcardId);

        void RemoveAllUserTransactions(Guid userId);
    }
}
