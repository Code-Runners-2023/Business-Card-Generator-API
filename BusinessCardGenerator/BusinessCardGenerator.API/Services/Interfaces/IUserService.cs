using BusinessCardGenerator.API.Data;

namespace BusinessCardGenerator.API.Services.Interfaces
{
    public interface IUserService
    {
        public List<User> GetAll();

        public User GetById(int id);

        public bool Add(User user);

        public bool Update(User user);

        public bool Remove(int id);
    }
}
