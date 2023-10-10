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
            builder.HasOne(yacht => yacht.YachtType).WithMany(yachtType => yachtType.Yachts).HasForeignKey(yacht => yacht.YachtTypeId);
            builder.Property(yacht => yacht.Name).HasColumnType("varchar(100)");
            builder.Property(yacht => yacht.Description).HasColumnType("varchar(255)").IsRequired(false);
            builder.Property(yacht => yacht.Manufacture).HasColumnType("varchar(100)");
            builder.Property(yacht => yacht.LOA).HasColumnType("float");
            builder.Property(yacht => yacht.BEAM).HasColumnType("float");
            builder.Property(yacht => yacht.DRAFT).HasColumnType("float");
            builder.Property(yacht => yacht.FuelCapacity).HasColumnType("varchar(20)");
            builder.Property(yacht => yacht.CreationDate).HasColumnType("datetime").HasDefaultValueSql("getDate()");
        }
    }
}