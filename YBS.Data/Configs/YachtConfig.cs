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
            builder.Property(yacht => yacht.Name).HasColumnType("varchar(100)");
            builder.Property(yacht => yacht.ImageURL).HasColumnType("varchar(max)").IsRequired(false);
            builder.Property(yacht => yacht.Description).HasColumnType("varchar(max)").IsRequired(false);
            builder.Property(yacht => yacht.Manufacturer).HasColumnType("varchar(100)");
            builder.Property(yacht => yacht.GrossTonnageUnit).HasColumnType("varchar(10)");
            builder.Property(yacht => yacht.RangeUnit).HasColumnType("varchar(20)");
            builder.Property(yacht => yacht.SpeedUnit).HasColumnType("varchar(20)");
            builder.Property(yacht => yacht.FuelCapacityUnit).HasColumnType("varchar(10)");
            builder.Property(yacht => yacht.LOA).HasColumnType("varchar(20)");
            builder.Property(yacht => yacht.BEAM).HasColumnType("varchar(20)");
            builder.Property(yacht => yacht.DRAFT).HasColumnType("varchar(20)");
            builder.Property(yacht => yacht.CreationDate).HasColumnType("datetime").HasDefaultValueSql("getDate()");
        }
    }
}