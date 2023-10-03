using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Models;

namespace YBS.Data.Configs
{
    public class YachtEntityTypeConfiguration : IEntityTypeConfiguration<Yacht>
    {
        public void Configure(EntityTypeBuilder<Yacht> builder)
        {
            builder.ToTable("Yacht");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).HasColumnType("varchar").HasMaxLength(100).IsRequired();
            builder.Property(x => x.Image).HasColumnType("varchar").HasMaxLength(255).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(255).IsRequired();
            builder.Property(x => x.Manufacturer).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Year).HasColumnType("int").IsRequired();
            builder.Property(x => x.LOA).HasColumnType("float").IsRequired();
            builder.Property(x => x.BEAM).HasColumnType("float").IsRequired();
            builder.Property(x => x.DRAFT).HasColumnType("float").IsRequired();
            builder.Property(x => x.FuelCapacity).HasColumnType("varchar").HasMaxLength(20).IsRequired();
            builder.Property(x => x.MaximumGuestLimit).HasColumnType("int").IsRequired();
            builder.Property(x => x.Cabin).HasColumnType("int").IsRequired();
        }
    }
}
