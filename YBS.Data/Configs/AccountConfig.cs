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
            builder.HasKey(account => account.Id);
            builder.Property(account => account.Id).ValueGeneratedOnAdd();
            builder.Property(account => account.Username).HasColumnType("varchar(50)");
            builder.HasIndex(account => account.Username).IsUnique();
            builder.HasIndex(account => account.Email).IsUnique();
            builder.Property(account => account.Email).HasColumnType("varchar(100)");
            builder.Property(account => account.Password).HasColumnType("varchar(500)");
            builder.Property(account => account.CreationDate).HasColumnType("datetime").HasDefaultValueSql("getDate()");
        }
    }
}