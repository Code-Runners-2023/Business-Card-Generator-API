using Microsoft.EntityFrameworkCore;

namespace BusinessCardGenerator.API.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<BusinessCard> BusinessCards { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Deposit> Deposits { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
    }
}
