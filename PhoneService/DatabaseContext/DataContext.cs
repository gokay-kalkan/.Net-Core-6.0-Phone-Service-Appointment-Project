using Microsoft.EntityFrameworkCore;
using PhoneService.Entities;

namespace PhoneService.DatabaseContext
{
    public class DataContext : DbContext
    {
       
        public DataContext(DbContextOptions options) : base(options)
        {
             
        }
        
        public DbSet<Appointment>Appointments { get; set; }
        public DbSet<Problem>Problems { get; set; }
        public DbSet<Dealer>Dealers { get; set; }
        public DbSet<Brand>Brands { get; set; }
        public DbSet<City>Cities { get; set; }
        public DbSet<Price>Prices { get; set; }
        public DbSet<Model>Models { get; set; }
        public DbSet<Admin>Admins { get; set; }
    }
}
