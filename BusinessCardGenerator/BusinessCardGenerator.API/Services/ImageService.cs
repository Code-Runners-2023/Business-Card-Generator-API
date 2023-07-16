using BusinessCardGenerator.API.Data;
using BusinessCardGenerator.API.Models.Image;
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

        public List<ImageAzureFileModel> RemoveAll(Guid userId)
        {
            List<Image> images = GetAll(userId);

            foreach(var image in images)
            {
                context.Images.Remove(image);
            }

            context.SaveChanges();

            List<ImageAzureFileModel> azureFiles = images.Select(image => new ImageAzureFileModel(image))
                                                         .ToList();

            return azureFiles;
        }
    }
}
