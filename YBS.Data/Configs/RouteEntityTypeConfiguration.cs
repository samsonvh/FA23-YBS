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
    public class RouteEntityTypeConfiguration : IEntityTypeConfiguration<Route>
    {
        public void Configure(EntityTypeBuilder<Route> builder)
        {
            builder.ToTable("Route");
            builder.HasKey(x => x.Id);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).HasMaxLength(255).IsRequired();
            builder.Property(x => x.Beginning).HasMaxLength(255).IsRequired();
            builder.Property(x => x.Destination).HasMaxLength(255).IsRequired();
            builder.Property(x => x.PickupTime).HasColumnType("time").IsRequired();
            builder.Property(x => x.StartingTime).HasColumnType("time").IsRequired();
            builder.Property(x => x.EndingTime).HasColumnType("time").IsRequired();
            builder.Property(x => x.DurationTime).HasColumnType("int").IsRequired();
            builder.Property(x => x.DurationUnit).HasColumnType("varchar").HasMaxLength(10).IsRequired();
            builder.Property(x => x.Type).HasColumnType("varchar").HasMaxLength(15).IsRequired();
            builder.Property(x => x.Status).HasColumnType("varchar").HasMaxLength(15).IsRequired();

            builder.HasOne(x => x.Company)
                .WithMany(c => c.Routes)
                .HasForeignKey(x => x.CompanyId);

        }
    }
}
