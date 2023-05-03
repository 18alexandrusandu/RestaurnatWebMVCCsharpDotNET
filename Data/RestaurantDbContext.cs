using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication5.Models;

namespace WebApplication5.Data
{
    public class RestaurantDbContext : DbContext
    {

      
        public RestaurantDbContext (DbContextOptions<RestaurantDbContext> options)
            : base(options)
        {
        }

        public DbSet<WebApplication5.Models.MenuItem> MenuItem { get; set; } = default!;

        public DbSet<WebApplication5.Models.Comanda> Comanda { get; set; }

        public DbSet<WebApplication5.Models.User> User { get; set; }

        public DbSet<WebApplication5.Models.ComandaItem> ComenziItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comanda>().HasKey(a => a.Id);
            modelBuilder.Entity<ComandaItem>().HasKey(a => a.Id);
            modelBuilder.Entity<MenuItem>().HasKey(a => a.Id);
            modelBuilder.Entity<User>().HasKey(a => a.Id);

            modelBuilder.Entity<ComandaItem>().HasOne<Comanda>().WithMany(a => a.produseList).HasForeignKey(a => a.ComandaId);
            modelBuilder.Entity<ComandaItem>().HasOne<MenuItem>().WithMany().IsRequired().HasForeignKey(a=>a.ItemId);

         

        }
    }

}
