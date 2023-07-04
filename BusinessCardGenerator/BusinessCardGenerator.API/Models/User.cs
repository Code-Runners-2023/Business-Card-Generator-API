﻿namespace BusinessCardGenerator.API.Models
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public int Age { get; set; }

        public string PasswordHash { get; set; }
    }
}