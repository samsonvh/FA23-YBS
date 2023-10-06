using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace YBS.Data.Configs
{
    public class MemberEntityTypeConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.ToTable("Member");
            builder.HasKey(member => member.Id);
            builder.Property(member => member.Id).ValueGeneratedOnAdd();
            builder.Property(member => member.FullName).HasMaxLength(100).IsRequired();
            builder.Property(member => member.DateOfBirth).HasColumnType("date").IsRequired();
            builder.Property(member => member.Nationality).HasMaxLength(100).HasColumnType("varchar").IsRequired();
            builder.Property(member => member.AvatarUrl).HasMaxLength(255).HasColumnType("varchar").IsRequired(false);
            builder.Property(member => member.Gender).IsRequired();
            builder.Property(member => member.Address).HasMaxLength(100).IsRequired();
            builder.Property(member => member.IdentityNumber).HasMaxLength(12).HasColumnType("varchar").IsRequired();
            builder.Property(member => member.MembershipStartDate).HasColumnType("date").IsRequired();
            builder.Property(member => member.MembershipExpiredDate).HasColumnType("date").IsRequired();
            builder.Property(member => member.MemberSinceDate).HasColumnType("date").IsRequired();
            builder.Property(member => member.LastModifiedDate).HasColumnType("date").HasDefaultValueSql("getDate()").IsRequired();
            builder.Property(company => company.Status).IsRequired();
        }
    }
}
