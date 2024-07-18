using Microsoft.EntityFrameworkCore;
using TelephoneShop.Models;

namespace TelephoneShop.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        { }

        public DbSet<Telephone> Telephone {  get; set; }

        public DbSet<Catalog> Catalog { get; set; }

        public DbSet<Cities> Cities { get; set; }

        public DbSet<CitiesToTelephoneCost> CitiesToTelephoneCost { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CitiesToTelephoneCost>().HasNoKey();
        }

    }
}
