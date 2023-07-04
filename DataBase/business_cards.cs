namespace CodeRunners.Database 
{
    public class BusinessCard 
    {
        public int Id { get; set; }
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public virtual User User { get; set; } 
        [string.length(50)] 
        public string Name { get; set; } 
        [string.length(50)]
        public string Position { get; set; } 
        [string.length(250)]
        public string Address { get; set; } 
        [string.length(1000)]
        public string Website { get; set; } 
        // [ForeignKey(nameof(Image))]
        public string Logo { get; set; } // path?? 
        [string.length(10)]
        public string Color { get; set; } 
    }
}