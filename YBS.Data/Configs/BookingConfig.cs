using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS.Data.Models;

namespace YBS.Data.Configs
{
    public class BookingConfig : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.ToTable("Booking");
            builder.HasKey(booking => booking.Id);
            builder.Property(booking => booking.Id).ValueGeneratedOnAdd();
            builder.Property(booking => booking.Note).HasColumnType("nvarchar(max)").IsRequired(false);
            builder.Property(booking => booking.MoneyUnit).HasColumnType("varchar(10)");
            builder.Property(company => company.CreationDate).HasColumnType("datetime").HasDefaultValueSql("getDate()");
            builder.Property(company => company.LastModifiedDate).HasColumnType("datetime").HasDefaultValueSql("getDate()");
             builder.HasOne(booking => booking.YachtType).WithMany(yachtType => yachtType.Bookings)
                                                                                    .HasForeignKey(booking => booking.YachtTypeId)
                                                                                    .OnDelete(DeleteBehavior.NoAction);
        
        }
    }
}