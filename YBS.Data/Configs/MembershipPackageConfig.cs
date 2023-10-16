using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS.Data.Models;

namespace YBS.Data.Configs
{
    public class MemberPackageConfig : IEntityTypeConfiguration<MembershipPackage>
    {
        public void Configure(EntityTypeBuilder<MembershipPackage> builder)
        {
            builder.ToTable("MembershipPackage");
            builder.HasKey(membershipPackage => membershipPackage.Id);
            builder.Property(membershipPackage => membershipPackage.Id).ValueGeneratedOnAdd();
            builder.Property(membershipPackage => membershipPackage.Name).HasColumnType("nvarchar(100)");
            builder.Property(membershipPackage => membershipPackage.Price).HasColumnType("float");
            builder.Property(membershipPackage => membershipPackage.Unit).HasColumnType("varchar(10)");
            builder.Property(membershipPackage => membershipPackage.Description).HasColumnType("nvarchar(255)").IsRequired(false);
            builder.Property(membershipPackage => membershipPackage.Point).HasColumnType("float");
            builder.Property(membershipPackage => membershipPackage.TimeUnit).HasColumnType("nvarchar(10)");
            builder.Property(membershipPackage => membershipPackage.CreationDate).HasColumnType("datetime").HasDefaultValueSql("getDate()");
            builder.Property(membershipPackage => membershipPackage.LastModifiedDate).HasColumnType("datetime").HasDefaultValueSql("getDate()");
        }
    }
}