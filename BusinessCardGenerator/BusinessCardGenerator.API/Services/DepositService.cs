using BusinessCardGenerator.API.Data;
using BusinessCardGenerator.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusinessCardGenerator.API.Services
{
    public class DepositService : IDepositService
    {
        private readonly ApplicationContext context;

        public DepositService(ApplicationContext context)
        {
            this.context = context;
        }

        public List<Deposit> GetAllUserDeposits(Guid userId)
            => context.Deposits
                      .Include(deposit => deposit.User)
                      .Where(deposit => deposit.UserId.Equals(userId))
                      .ToList();

        public Deposit GetById(Guid depositId)
            => context.Deposits
                      .Include(deposit => deposit.User)
                      .FirstOrDefault(deposit => deposit.Id.Equals(depositId));

        public bool CheckIfUserIsOwner(Guid userId, Guid depositId)
            => context.Deposits
                      .Include(deposit => deposit.User)
                      .Any(deposit => deposit.Id.Equals(depositId) && deposit.UserId.Equals(userId));

        public bool Add(Deposit deposit)
        {
            if (deposit.Amount <= 0)
                return false;

            context.Deposits.Add(deposit);
            context.SaveChanges();

            return true;
        }

        public void RemoveAllUserDeposits(Guid userId)
        {
            GetAllUserDeposits(userId).ForEach(deposit => context.Deposits.Remove(deposit));
            context.SaveChanges();
        }
    }
}
