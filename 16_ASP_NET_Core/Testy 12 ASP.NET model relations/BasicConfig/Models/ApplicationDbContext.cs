using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicConfig.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>
        options) : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientsProducts> CliPro { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientsProducts>()
                .HasKey(c => new { c.ProductID, c.ClientID });
            modelBuilder.Entity<ClientsProducts>()
                .HasOne(pt => pt.Product)
                .WithMany(p => p.CPs)
                .HasForeignKey(pt => pt.ProductID);

            modelBuilder.Entity<ClientsProducts>()
                .HasOne(pt => pt.Client)
                .WithMany(t => t.CPs)
                .HasForeignKey(pt => pt.ClientID);
        }

    }
}
