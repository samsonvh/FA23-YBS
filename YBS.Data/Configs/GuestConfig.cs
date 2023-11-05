using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS.Data.Models;

namespace YBS.Data.Configs
{
    public class GuestConfig : IEntityTypeConfiguration<Guest>
    {
        public void Configure(EntityTypeBuilder<Guest> builder)
        {
            builder.ToTable("Guest");
            builder.HasKey(guest => guest.Id);
            builder.Property(guest => guest.Id).ValueGeneratedOnAdd();
            builder.Property(guest => guest.FullName).HasColumnType("nvarchar(100)");
            builder.Property(guest => guest.DateOfBirth).HasColumnType("date");
            builder.Property(guest => guest.IdentityNumber).HasColumnType("varchar(12)");
            builder.Property(guest => guest.PhoneNumber).HasColumnType("varchar(12)");
        }
    }
}