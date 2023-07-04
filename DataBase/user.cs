namespace CodeRunners.Database
{
    public class User 
    {
        public int Id { get; set; }
        [string.length(50)] 
        public string Name { get; set; } 
        [string.length(50)]
        public string Email { get; set; } 
        [string.length(1500)]
        public string Password { get; set; } 
        public string Phone { get; set; } 
        public double Balance { get; set; } 
    }
}