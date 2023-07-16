using BusinessCardGenerator.API.Data;

namespace BusinessCardGenerator.API.Services.Interfaces
{
    public interface IDepositService
    {
        List<Deposit> GetAllUserDeposits(Guid userId);

        Deposit GetById(Guid depositId);

        bool CheckIfUserIsOwner(Guid userId, Guid depositId);

        bool Add(Deposit deposit);

        void RemoveAllUserDeposits(Guid userId);
    }
}
