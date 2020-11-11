using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using MyBatiment.Core.Models;

namespace MyBatiment.Data.Configuration
{
    public class ProjectItemConfiguration : IEntityTypeConfiguration<ProjectItem>
    {
        public void Configure(EntityTypeBuilder<ProjectItem> builder)
        {
            builder
                .HasKey(a => a.Id);

            builder
                .Property(m => m.ProjectName)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .ToTable("ProjectItems");
        }
    }
}
