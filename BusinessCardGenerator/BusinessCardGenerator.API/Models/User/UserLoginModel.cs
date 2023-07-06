using System.ComponentModel.DataAnnotations;

namespace BusinessCardGenerator.API.Models.User
{
    public class UserLoginModel
    {
        [EmailAddress(ErrorMessage = "Invalid email address!")]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
