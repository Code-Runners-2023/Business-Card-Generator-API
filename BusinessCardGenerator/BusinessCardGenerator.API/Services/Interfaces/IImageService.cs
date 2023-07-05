using BusinessCardGenerator.API.Data;

namespace BusinessCardGenerator.API.Services.Interfaces
{
    public interface IImageService
    {
        List<Image> GetAll(Guid userId);

        Image GetById(Guid userId, Guid imageId);

        string GetFromCloud(Guid imageId);

        void Add(Image image);

        void SaveInCloud(Guid imageId, IFormFile file);

        Image Remove(Guid userId, Guid imageId);

        string DeleteFromCloud(Guid imageId);
    }
}
