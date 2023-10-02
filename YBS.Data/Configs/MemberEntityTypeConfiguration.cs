﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS.Data.Extensions.Enums;
using YBS.Data.Models;

namespace YBS.Data.Configs
{
    public class MemberEntityTypeConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.ToTable("Member");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.FullName).HasMaxLength(150).IsRequired();
            builder.Property(x => x.DateOfbirth).HasColumnType("date").IsRequired();
            builder.Property(x => x.Nationality).HasMaxLength(100).HasColumnType("varchar").IsRequired();
            builder.Property(x => x.Gender).HasColumnType("varchar").HasMaxLength(15).IsRequired()
            .HasConversion(
                x => x.ToString(),
                x => (Gender)Enum.Parse(typeof(Gender), x)
            );
            builder.Property(x => x.Address).HasMaxLength(100).HasColumnType("varchar").IsRequired();
            builder.Property(x => x.IdentityNumber).HasMaxLength(12).HasColumnType("varchar").IsRequired();
            builder.HasIndex(x => x.IdentityNumber).IsUnique();
            builder.Property(x => x.MembershipStartDate).HasColumnType("datetime").IsRequired();
            builder.Property(x => x.MembershipExpiredDate).HasColumnType("datetime").IsRequired();
            builder.Property(x => x.MemberSinceDate).HasColumnType("date").IsRequired();
            builder.Property(x => x.Status).HasColumnType("varchar").HasMaxLength(15).IsRequired()
            .HasConversion(
                x => x.ToString(),
                x => (MemberStatus)Enum.Parse(typeof(MemberStatus), x)
            );
        }
    }
}