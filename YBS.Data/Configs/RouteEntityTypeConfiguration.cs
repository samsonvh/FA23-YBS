﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS.Data.Enums;
using YBS.Data.Models;

namespace YBS.Data.Configs
{
    public class RouteEntityTypeConfiguration : IEntityTypeConfiguration<Route>
    {
        public void Configure(EntityTypeBuilder<Route> builder)
        {
            builder.ToTable("Route");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).HasMaxLength(255).IsRequired();
            builder.Property(x => x.Beginning).HasMaxLength(255).IsRequired();
            builder.Property(x => x.Destination).HasMaxLength(255).IsRequired();
            builder.Property(x => x.PickupTime).HasColumnType("time").IsRequired();
            builder.Property(x => x.StartingTime).HasColumnType("time").IsRequired();
            builder.Property(x => x.EndingTime).HasColumnType("time").IsRequired();
            builder.Property(x => x.DurationTime).HasColumnType("int").IsRequired();
            builder.Property(x => x.DurationUnit).HasMaxLength(10).HasColumnType("varchar").IsRequired();
            builder.Property(x => x.Status).HasMaxLength(15).IsRequired();
          
             builder.HasOne(x => x.Company)
                .WithMany(c => c.Routes)
                .HasForeignKey(x => x.CompanyId);

            builder.HasMany(x => x.Activities)
                .WithOne(a => a.Route)
                .HasForeignKey(a => a.RouteId);
        }
    }
}
