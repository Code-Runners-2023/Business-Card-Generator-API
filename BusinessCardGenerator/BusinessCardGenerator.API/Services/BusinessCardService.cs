using BusinessCardGenerator.API.Data;
using BusinessCardGenerator.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public string GetLogoFromCloud(Guid bcardId)
        {
            Console.WriteLine("[TODO] BusinessCardService -> GetLogoFromCloud method is not implemented yet!");

            return $"{GetById(bcardId)?.LogoPath}";
        }

        public string GetLogoPathInCloud(Guid bcardId)
        {
            Console.WriteLine("[TODO] BusinessCardService -> GetLogoPathInCloud method is not implemented yet!");

            return $"{GetById(bcardId)?.LogoPath}";
        }

        public void SaveLogoInCloud(Guid bcardId, IFormFile file)
        {
            Console.WriteLine("[TODO] BusinessCardService -> SaveLogoInCloud method is not implemented yet!");
        }

        public string DeleteLogoFromCloud(Guid bcardId)
        {
            Console.WriteLine("[TODO] BusinessCardService -> DeleteLogoFromCloud method is not implemented yet!");

            return $"{GetById(bcardId)?.LogoPath}";
        }
    }
}
