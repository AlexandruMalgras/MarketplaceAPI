using Marketplace.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Marketplace.Configurations
{
    public class DatabaseConfiguration : IdentityDbContext
    {
        private IConfiguration configuration;

        public DbSet<Users> Users { get; set; }
        public DbSet<UserActions> UserActions { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
        public DbSet<Reviews> Reviews { get; set; }

        public DatabaseConfiguration() : base()
        {
            this.configuration = new ConfigurationBuilder().AddJsonFile(Environment.CurrentDirectory + "/appsettings.json").Build();

            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbName = this.configuration.GetSection("Database").GetValue<string>("Name");

            optionsBuilder
                .UseSqlServer(@"Server=" + dbName + ";Database=Marketplace;Trusted_Connection=True;TrustServerCertificate=true;");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Orders>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)  
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Products>()
                .HasOne(o => o.User)
                .WithMany(u => u.Products)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
