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
                      .Where(image => image.UserId.Equals(userId))
                      .ToList();

        public Image GetById(Guid imageId)
            => context.Images
                      .Include(image => image.User)
                      .FirstOrDefault(image => image.Id.Equals(imageId));

        public bool CheckIfUserIsOwner(Guid userId, Guid imageId)
            => context.Images
                      .Include(image => image.User)
                      .Any(image => image.Id.Equals(imageId) && image.UserId.Equals(userId));

        public void Add(Image image)
        {
            context.Images.Add(image);
            context.SaveChanges();
        }

        public Image Remove(Guid userId, Guid imageId)
        {
            Image image = GetById(imageId);

            if (image == null || image.UserId != userId)
                return null;

            context.Images.Remove(image);
            context.SaveChanges();

            return image;
        }

        public string GetFromCloud(Guid imageId)
        {
            Console.WriteLine("[TODO] ImageService -> GetFromCloud method is not implemented yet!");
            
            return $"{GetById(imageId)?.Path}";
        }

        public void SaveInCloud(Guid imageId, IFormFile file)
        {
            Console.WriteLine("[TODO] ImageService -> SaveInCloud method is not implemented yet!");
        }

        public string DeleteFromCloud(Guid imageId)
        {
            Console.WriteLine("[TODO] ImageService -> DeleteFromCloud method is not implemented yet!");

            return $"{GetById(imageId)?.Path}";
        }
    }
}
