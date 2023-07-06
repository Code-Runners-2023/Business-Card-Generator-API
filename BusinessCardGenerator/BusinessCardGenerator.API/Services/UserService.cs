using BusinessCardGenerator.API.Data;
using BusinessCardGenerator.API.Models.User;
using BusinessCardGenerator.API.Services.Interfaces;

namespace BusinessCardGenerator.API.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationContext context;

        public UserService(ApplicationContext context)
        {
            this.context = context;
        }

        public List<User> GetAll()
            => context.Users.ToList();

        public User GetById(Guid id)
            => context.Users.FirstOrDefault(user => user.Id.Equals(id));

        public User GetByEmailAndPassword(string email, string password)
            => context.Users.FirstOrDefault(user => user.Email == email && user.Password == password);

        public bool VerifyLogin(UserLoginModel login)
            => GetByEmailAndPassword(login.Email, login.Password) != null;

        public void Add(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
        }

        public void Update(User user)
        {
            context.Users.Update(user);
            context.SaveChanges();
        }

        public User Remove(Guid id)
        {
            User user = GetById(id);

            if (user == null)
                return null;

            context.Users.Remove(user);
            context.SaveChanges();

            return user;
        }
    }
}
