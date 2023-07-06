using BusinessCardGenerator.API.Data;
using BusinessCardGenerator.API.Models.User;

namespace BusinessCardGenerator.API.Services.Interfaces
{
    public interface IUserService
    {
        List<User> GetAll();

        User GetById(Guid id);

        User GetByEmail(string email);

        User GetByEmailAndPassword(string email, string password);

        bool VerifyLogin(UserLoginModel login);

        bool IsUserRegisteredWithEmail(string email);

        void Add(User user);

        void Update(User user);

        User Remove(Guid id);
    }
}
