using BusinessCardGenerator.API.Data;
using BusinessCardGenerator.API.Models.Image;

namespace BusinessCardGenerator.API.Services.Interfaces
{
    public interface IImageService
    {
        List<Image> GetAll(Guid userId);

        Image GetById(Guid imageId);

        bool CheckIfUserIsOwner(Guid userId, Guid imageId);

        void Add(Image image);

        Image Remove(Guid userId, Guid imageId);

        List<ImageAzureFileModel> RemoveAll(Guid userId);
    }
}
