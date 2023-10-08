using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS.Data.Models;

namespace YBS.Data.Configs
{
    public class AccountConfig : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Account");
            builder.HasKey(account => account.ID);
            builder.HasOne(account => account.Role)
            .WithMany(role => role.Accounts).HasForeignKey(account => account.RoleID);
            builder.Property(account => account.Username).HasColumnType("varchar(50)");
            builder.HasIndex(account => account.Username).IsUnique();
            builder.Property(account => account.Email).HasColumnType("varchar(100)");
            builder.Property(account => account.Password).HasColumnType("varchar(500)");
            builder.Property(account => account.CreationDate).HasColumnType("datetime").HasDefaultValueSql("getDate()");
        }
    }
}