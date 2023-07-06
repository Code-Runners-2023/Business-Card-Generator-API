using BusinessCardGenerator.API.Data;

namespace BusinessCardGenerator.API.Services.Interfaces
{
    public interface IBusinessCardService
    {
        List<BusinessCard> GetAll(Guid userId);

        BusinessCard GetById(Guid bcardId);

        void Add(BusinessCard bcard);

        void Update(BusinessCard bcard);

        BusinessCard Remove(Guid userId, Guid bcardId);
    }
}
