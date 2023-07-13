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

        public void RemoveAll(Guid userId)
        {
            GetAll(userId).ForEach(image => context.Images.Remove(image));
            context.SaveChanges();
        }
    }
}
