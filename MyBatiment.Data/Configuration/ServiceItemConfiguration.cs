using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MyBatiment.Core.Models;

namespace MyBatiment.Data.Configuration
{
    class ServiceItemConfiguration : IEntityTypeConfiguration<ServiceItem>
    {
        public void Configure(EntityTypeBuilder<ServiceItem> builder)
        {
            builder
                .HasKey(a => a.Id);

            builder
                .Property(m => m.ServiceName)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .ToTable("ServiceItems");
        }
    }
}
