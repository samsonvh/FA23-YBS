using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Metadata.Ecma335;
using YBS.Data.Models;


namespace YBS.Data.Configs
{
    public class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Account");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Email).HasMaxLength(200).HasColumnType("varchar").IsRequired();
            builder.HasIndex(x => x.Email).IsUnique();
            builder.Property(x => x.Password).HasMaxLength(500).HasColumnType("varchar").IsRequired();
            builder.HasOne(x => x.Role).WithMany(x => x.Accounts).HasForeignKey(x => x.RoleID);
            builder.Property(x => x.RoleID).IsRequired();
            builder.Property(x => x.CreationDate).HasColumnType("date").HasDefaultValueSql("getDate()").IsRequired();
        }
    }
}
