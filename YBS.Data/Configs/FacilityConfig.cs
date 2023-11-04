using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS.Data.Models;

namespace YBS.Data.Configs
{
    public class FacilityConfig : IEntityTypeConfiguration<Facility>
    {
        public void Configure(EntityTypeBuilder<Facility> builder)
        {
            builder.ToTable("Facility");
            builder.HasIndex(facility => facility.Id);
            builder.Property(facility => facility.Id).ValueGeneratedOnAdd();
            builder.Property(facility => facility.Title).HasColumnType("nvarchar(100)");
            builder.Property(facility => facility.Description).HasColumnType("nvarchar(max)");
        }
    }
}