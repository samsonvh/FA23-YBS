using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS.Data.Models;

namespace YBS.Data.Configs
{
    public class TripConfig : IEntityTypeConfiguration<Trip>
    {
        public void Configure(EntityTypeBuilder<Trip> builder)
        {
            builder.ToTable("Trip");
            builder.HasKey(trip => trip.Id);
            builder.Property(trip => trip.Id).ValueGeneratedOnAdd();
            builder.Property(trip => trip.Name).HasColumnType("nvarchar(100)");
            builder.Property(route => route.ExpectedPickupTime).HasColumnType("datetime").IsRequired();
            builder.Property(route => route.ExpectedStartingTime).HasColumnType("datetime").IsRequired();
            builder.Property(route => route.ExpectedEndingTime).HasColumnType("datetime").IsRequired();
            builder.Property(route => route.ExpectedDurationTime).HasColumnType("int").IsRequired();
            builder.Property(route => route.DurationUnit).HasColumnType("varchar").HasMaxLength(10).IsRequired();
        }
    }
}