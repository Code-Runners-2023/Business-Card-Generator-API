using BusinessCardGenerator.API.Data;
using System.ComponentModel.DataAnnotations;

namespace BusinessCardGenerator.API.Models
{
    public class UserCompressedInfoModel
    {
        public UserCompressedInfoModel(User user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Phone = user.Phone;
        }

        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
    }
}
