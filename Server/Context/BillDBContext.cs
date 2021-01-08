using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.Extensions.Options;
using IdentityServer4.EntityFramework.Options;
using ERacuni.Shared.Models;

namespace ERacuni.Server.Context
{
    public class BillDBContext : ApiAuthorizationDbContext<IdentityUser>
    {
        public BillDBContext(DbContextOptions<BillDBContext> dbContextOptions ,
            IOptions<OperationalStoreOptions> options) : base(dbContextOptions, options) { }

        public DbSet<Bill> Bills { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Bill>().HasKey(u => u.barcode);
            builder.Entity<Bill>().HasIndex(u => u.barcode).IsUnique();
            builder.Entity<Product>().HasKey(u => u.ID);


            base.OnModelCreating(builder);
            builder.Entity<User>().HasData(new User
            {
                Id = Guid.NewGuid().ToString(), //nasledjuje od IdentityUser-a
                firstName = "Almedin",
                lastName = "Ljajic",
                UserName = "proyectant" //nasledjuje od IdentityUser-a
            });

        }

    }
}
