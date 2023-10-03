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
    public class ActivityEntityTypeConfiguration : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder)
        {
            builder.ToTable("Activity");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.StartCoordinate).HasColumnType("varchar").HasMaxLength(50);
            builder.Property(x => x.EndCoordinate).HasColumnType("varchar").HasMaxLength(50);
            builder.Property(x => x.Name).HasColumnType("varchar").HasMaxLength(255).IsRequired();
            builder.Property(x => x.Description).HasColumnType("varchar").HasMaxLength(50).IsRequired();
            builder.Property(x => x.OccuringTime).HasColumnType("time").IsRequired();
            builder.Property(x => x.OrderIndex).HasColumnType("int").IsRequired();
            builder.Property(x => x.Status).HasMaxLength(15).IsRequired();
        }
    }
}
