using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCardGenerator.API.Data
{
    public class Image
    {
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public User User { get; set; }

        public string Path { get; set; }
    }
}