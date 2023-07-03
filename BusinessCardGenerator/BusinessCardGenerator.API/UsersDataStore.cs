using BusinessCardGenerator.API.Models;

namespace BusinessCardGenerator.API
{
    public class UsersDataStore
    {
        public static UsersDataStore Current { get; set; } = new UsersDataStore();

        public List<User> Users { get; private set; }

        public UsersDataStore()
        {
            Users = new List<User>();
            GenerateUsers();
        }

        private void GenerateUsers()
        {
            for (int i = 0; i < 10; i++)
            {
                Users.Add(new User()
                {
                    Id = i,
                    FirstName = $"Ivan #{i}",
                    LastName = $"Ivanov #{i}",
                    Age = i * 2 + 7,
                    PasswordHash = $"ZlatenPrinter12{i}"
                });
            }
        }
    }
}
