using BusinessCardGenerator.API.Data;
using BusinessCardGenerator.API.Services.Interfaces;

namespace BusinessCardGenerator.API.Services
{
    public class BusinessCardService : IBusinessCardService
    {
        private readonly ApplicationContext context;

        public BusinessCardService(ApplicationContext context)
        {
            this.context = context;
        }

        public List<BusinessCard> GetAll(Guid userId)
            => 

        public BusinessCard GetById(Guid bcardId)
        {
            throw new NotImplementedException();
        }

        public void Add(BusinessCard bcard)
        {
            throw new NotImplementedException();
        }

        public void Update(BusinessCard bcard)
        {
            throw new NotImplementedException();
        }

        public BusinessCard Remove(Guid userId, Guid bcardId)
        {
            throw new NotImplementedException();
        }
    }
}
