using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS.Data.Models;

namespace YBS.Data.Configs
{
    public class MembershipRegistrationConfig : IEntityTypeConfiguration<MembershipRegistration>
    {
        public void Configure(EntityTypeBuilder<MembershipRegistration> builder)
        {
            builder.ToTable("MembershipRegistration");
            builder.HasKey(membershipRegistration => membershipRegistration.Id);
            builder.Property(membershipRegistration => membershipRegistration.Id).ValueGeneratedOnAdd();
            builder.HasOne(membershipRegistration => membershipRegistration.Member).WithMany(member => member.MembershipRegistrations)
                                                                                    .HasForeignKey(membershipRegistration => membershipRegistration.MemberId)
                                                                                    .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(membershipRegistration => membershipRegistration.MembershipPackage).WithMany(membershipPackage => membershipPackage.MembershipRegistrations)
                                                                                                .HasForeignKey(membershipRegistration => membershipRegistration.MembershipPackageId);
            builder.Property(membershipRegistration => membershipRegistration.Amount).HasColumnType("float");
            builder.Property(membershipRegistration => membershipRegistration.MoneyUnit).HasColumnType("varchar(10)");
            builder.Property(membershipRegistration => membershipRegistration.DateRegistered).HasColumnType("datetime");
        }
    }
}