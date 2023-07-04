using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BusinessCardGenerator.API.Data
{
    public class User
    {
        public int Id { get; set; }

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