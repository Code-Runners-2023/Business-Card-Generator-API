using BusinessCardGenerator.API.Data;

namespace BusinessCardGenerator.API.Services.Interfaces
{
    public interface IUserService
    {
        List<User> GetAll();

        User GetById(Guid id);

        void Add(User user);

        void Update(User user);

        User Remove(Guid id);
    }
}
