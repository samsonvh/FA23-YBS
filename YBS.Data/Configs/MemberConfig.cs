using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS.Data.Models;

namespace YBS.Data.Configs
{
    public class MemberConfig : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.ToTable("Member");
            builder.HasKey(member => member.ID);
            builder.HasOne(member => member.Account).WithOne(account => account.Member)
            .HasForeignKey<Member>(member => member.AccountID);
            builder.HasOne(member => member.MembershipPackage).WithMany(membershipPackage => membershipPackage.Members)
            .HasForeignKey(member => member.MembershipPackageID).IsRequired(false);
            builder.Property(member => member.FullName).HasColumnType("nvarchar(100)");
            builder.Property(member => member.DOB).HasColumnType("date");
            builder.Property(member => member.Nationality).HasColumnType("varchar(100)");
            builder.Property(member => member.Avatar).HasColumnType("varchar(255)");
            builder.Property(member => member.Address).HasColumnType("nvarchar(200)");
            builder.Property(member => member.IdentityNumber).HasColumnType("varchar(15)");
            builder.Property(member => member.MembershipStartDate).HasColumnType("datetime").HasDefaultValueSql("getDate()");
            builder.Property(member => member.MembershipExpiredDate).HasColumnType("datetime");
            builder.Property(member => member.MembershipSinceDate).HasColumnType("datetime").HasDefaultValueSql("getDate()");
            builder.Property(member => member.LastModifiedDate).HasColumnType("datetime").HasDefaultValueSql("getDate()");
        }
    }
}