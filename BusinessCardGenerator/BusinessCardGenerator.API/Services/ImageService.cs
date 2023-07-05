using BusinessCardGenerator.API.Data;
using BusinessCardGenerator.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusinessCardGenerator.API.Services
{
    public class ImageService : IImageService
    {
        private readonly ApplicationContext context;

        public ImageService(ApplicationContext context)
        {
            this.context = context;
        }

        public List<Image> GetAll(Guid userId)
            => context.Images
                      .Include(image => image.User)
                      .Where(image => image.UserId == userId)
                      .ToList();

        public Image GetById(Guid userId, Guid imageId)
            => context.Images
                      .Include(image => image.User)
                      .FirstOrDefault(image => image.Id == imageId && image.UserId == userId);

        public string GetFromCloud(Guid imageId)
        {
            return $"[TODO] ImageService -> GetFromCloud method is not implemented yet!";
        }

        public void Add(Image image)
        {
            context.Images.Add(image);
            context.SaveChanges();
        }

        public void SaveInCloud(Guid imageId, IFormFile file)
        {
            Console.WriteLine("[TODO] ImageService -> SaveInCloud method is not implemented yet!");
        }

        public Image Remove(Guid userId, Guid imageId)
        {
            Image image = GetById(userId, imageId);

            if (image == null)
                return null;

            context.Images.Remove(image);
            context.SaveChanges();

            return image;
        }

        public string DeleteFromCloud(Guid imageId)
        {
            return "[TODO] ImageService -> DeleteFromCloud method is not implemented yet!";
        }
    }
}
