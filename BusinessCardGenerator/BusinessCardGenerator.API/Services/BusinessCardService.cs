using BusinessCardGenerator.API.Data;
using BusinessCardGenerator.API.Models.Image;
using BusinessCardGenerator.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

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
            => context.BusinessCards
                      .Include(bcard => bcard.User)
                      .Where(bcard => bcard.UserId.Equals(userId))
                      .ToList();

        public BusinessCard GetById(Guid bcardId)
            => context.BusinessCards
                      .Include(bcard => bcard.User)
                      .FirstOrDefault(bcard => bcard.Id.Equals(bcardId));

        public bool CheckIfUserIsOwner(Guid userId, Guid bcardId)
            => context.BusinessCards
                      .Include(bcard => bcard.User)
                      .Any(bcard => bcard.Id.Equals(bcardId) && bcard.UserId.Equals(userId));

        public void Add(BusinessCard bcard)
        {
            context.BusinessCards.Add(bcard);
            context.SaveChanges();
        }

        public void Update(BusinessCard bcard)
        {
            context.BusinessCards.Update(bcard);
            context.SaveChanges();
        }

        public BusinessCard Remove(Guid userId, Guid bcardId)
        {
            BusinessCard bcard = GetById(bcardId);

            if (bcard == null || bcard.UserId != userId)
                return null;

            context.BusinessCards.Remove(bcard);
            context.SaveChanges();

            return bcard;
        }

        public List<ImageAzureFileModel> RemoveAll(Guid userId)
        {
            List<BusinessCard> bcards = GetAll(userId);

            foreach (var bcard in bcards)
            {
                context.BusinessCards.Remove(bcard);
            }

            context.SaveChanges();

            List<ImageAzureFileModel> azureFiles = bcards.Select(bcard => new ImageAzureFileModel(bcard))
                                                         .ToList();

            return azureFiles;
        }
    }
}
