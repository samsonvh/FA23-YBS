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
            builder.HasKey(member => member.Id);
            builder.Property(member => member.Id).ValueGeneratedOnAdd();
            builder.Property(member => member.FullName).HasColumnType("nvarchar(100)");
            builder.Property(member => member.DateOfBirth).HasColumnType("date");
            builder.Property(member => member.PhoneNumber).HasColumnType("varchar(15)");
            builder.Property(member => member.Nationality).HasColumnType("nvarchar(100)");
            builder.Property(member => member.AvatarURL).HasColumnType("nvarchar(max)").IsRequired(false);
            builder.Property(member => member.Address).HasColumnType("nvarchar(200)");
            builder.Property(member => member.IdentityNumber).HasColumnType("varchar(15)");
            builder.Property(member => member.MembershipStartDate).HasColumnType("datetime").HasDefaultValueSql("getDate()");
            builder.Property(member => member.MembershipExpiredDate).HasColumnType("datetime");
            builder.Property(member => member.MembershipSinceDate).HasColumnType("datetime").HasDefaultValueSql("getDate()");
            builder.Property(member => member.LastModifiedDate).HasColumnType("datetime").HasDefaultValueSql("getDate()");
        }
    }
}