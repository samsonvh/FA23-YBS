using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS.Data.Models;

namespace YBS.Data.Configs
{
    public class BookingServicePackageConfig : IEntityTypeConfiguration<BookingServicePackage>
    {
        public void Configure(EntityTypeBuilder<BookingServicePackage> builder)
        {
            builder.ToTable("BookingServicePackage");
            builder.HasKey(bookingServicePackage => bookingServicePackage.Id);
            builder.Property(bookingServicePackage => bookingServicePackage.Id).ValueGeneratedOnAdd();
            builder.HasOne(bookingServicePackage => bookingServicePackage.Booking).WithMany(booking => booking.BookingServicePackages).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(bookingServicePackage => bookingServicePackage.ServicePackage).WithMany(servicePackage => servicePackage.BookingServicePackages).OnDelete(DeleteBehavior.NoAction);
        }
    }
}