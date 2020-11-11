using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MyBatiment.Core.Models;

namespace MyBatiment.Data.Configuration
{
    public class OwnerConfiguration : IEntityTypeConfiguration<Owner>
    {
        public void Configure(EntityTypeBuilder<Owner> builder)
        {
            builder
                .HasKey(a => a.Id);

            builder
                .Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .ToTable("Owners");
        }
    }
}
