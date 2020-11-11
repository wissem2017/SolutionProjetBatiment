using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;

using MyBatiment.Core.Models;
using MyBatiment.Data.Configuration;

namespace MyBatiment.Data
{
   public class MyDbContext : DbContext
    {
        public DbSet<Owner> Owners { get; set; }
        public DbSet<ProjectItem> ProjectItems { get; set; }

        public DbSet<ServiceItem> ServiceItems { get; set; }

        public DbSet<User> Users { get; set; }

        public MyDbContext(DbContextOptions<MyDbContext> options)
       : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new OwnerConfiguration());

            builder.ApplyConfiguration(new ProjectItemConfiguration());

            builder.ApplyConfiguration(new ServiceItemConfiguration());

            builder.ApplyConfiguration(new UserConfiguration());

            base.OnModelCreating(builder);

            builder.Entity<Owner>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            builder.Entity<ServiceItem>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            builder.Entity<ProjectItem>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            builder.Entity<User>().Property(x => x.Id).HasDefaultValueSql("NEWID()");

            builder.Entity<Owner>().HasData(
                new Owner()
                {
                    Id = Guid.NewGuid(),
                    Title = "Bâtiment",
                    Adress = "Paris 93600",
                    Description = " ",
                    Image = "intro - bg.jpg",
                    Tel = "0033 12 12 12 12 12"
                }
                );

        }
    }
}
