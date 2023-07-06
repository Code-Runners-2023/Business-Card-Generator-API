using BusinessCardGenerator.API.Models.User;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCardGenerator.API.Data
{
    public class User
    {
        public User() { }

        public User(UserInputModel input)
        {
            Id = Guid.NewGuid();
            Balance = 0;

            FirstName = input.FirstName;
            LastName = input.LastName;
            Email = input.Email;
            Phone = input.Phone;
            Password = input.Password;
        }

        public void ApplyChanges(UserInputModel model)
        {
            FirstName = model.FirstName;
            LastName = model.LastName;
            Email = model.Email;
            Phone = model.Phone;
            Password = model.Password;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [StringLength(100, ErrorMessage = "Too long first name! Max length is 100 characters!")]
        public string FirstName { get; set; }

        [StringLength(100, ErrorMessage = "Too long last name! Max length is 100 characters!")]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address!")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number!")]
        public string Phone { get; set; }

        public string Password { get; set; }

        public double Balance { get; set; }
    }
}