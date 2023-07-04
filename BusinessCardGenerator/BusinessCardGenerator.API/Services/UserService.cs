using BusinessCardGenerator.API.Data;
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

        public User GetById(int id)
            => context.Users.FirstOrDefault(user => user.Id == id);

        public bool Add(User user)
        {
            if (user == null)
                return false;

            context.Users.Add(user);
            context.SaveChanges();

            return true;
        }

        public bool Update(User user)
        {
            if (user == null)
                return false;

            context.Users.Update(user);
            context.SaveChanges();

            return true;
        }

        public User Remove(int id)
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
