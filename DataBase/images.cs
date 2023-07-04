namespace CodeRunners.Database
{
    public class Image
    {
        public int Id { get; set; }
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public virtual User User { get; set; } 
        [string.length(50)]
        public string Blob { get; set; } // path?? 
    }
}