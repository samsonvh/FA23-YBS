using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS.Data.Models;

namespace YBS.Data.Configs
{
    public class YachtConfig : IEntityTypeConfiguration<Yacht>
    {
        public void Configure(EntityTypeBuilder<Yacht> builder)
        {
            builder.ToTable("Yacht");
            builder.HasKey(yacht => yacht.Id);
            builder.Property(yacht => yacht.Id).ValueGeneratedOnAdd();
            builder.Property(yacht => yacht.Name).HasColumnType("nvarchar(100)");
            builder.Property(yacht => yacht.ImageURL).HasColumnType("nvarchar(max)").IsRequired(false);
            builder.Property(yacht => yacht.Description).HasColumnType("nvarchar(max)").IsRequired(false);
            builder.Property(yacht => yacht.Manufacturer).HasColumnType("nvarchar(100)");
            builder.Property(yacht => yacht.GrossTonnageUnit).HasColumnType("nvarchar(10)");
            builder.Property(yacht => yacht.RangeUnit).HasColumnType("nvarchar(20)");
            builder.Property(yacht => yacht.SpeedUnit).HasColumnType("nvarchar(20)");
            builder.Property(yacht => yacht.FuelCapacityUnit).HasColumnType("nvarchar(10)");
            builder.Property(yacht => yacht.LOA).HasColumnType("nvarchar(20)");
            builder.Property(yacht => yacht.BEAM).HasColumnType("nvarchar(20)");
            builder.Property(yacht => yacht.DRAFT).HasColumnType("nvarchar(20)");
            builder.Property(yacht => yacht.CreationDate).HasColumnType("datetime").HasDefaultValueSql("getDate()");
        }
    }
}