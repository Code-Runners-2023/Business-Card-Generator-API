using BusinessCardGenerator.API.Data;
using BusinessCardGenerator.API.Models.Image;

namespace BusinessCardGenerator.API.Services.Interfaces
{
    public interface IBusinessCardService
    {
        List<BusinessCard> GetAll(Guid userId);

        BusinessCard GetById(Guid bcardId);

        bool CheckIfUserIsOwner(Guid userId, Guid bcardId);

        void Add(BusinessCard bcard);

        void Update(BusinessCard bcard);

        BusinessCard Remove(Guid userId, Guid bcardId);

        List<ImageAzureFileModel> RemoveAll(Guid userId);
    }
}
