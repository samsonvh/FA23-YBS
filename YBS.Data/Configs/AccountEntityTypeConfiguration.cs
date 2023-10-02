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
            builder.Property(x => x.Username).HasMaxLength(150).HasColumnType("varchar").IsRequired();
            builder.Property(x => x.Email).HasMaxLength(200).HasColumnType("varchar").IsRequired();
            builder.HasIndex(x => x.Email).IsUnique();
            builder.Property(x => x.Password).HasMaxLength(500).HasColumnType("varchar").IsRequired();
            builder.Property(x => x.Role).HasMaxLength(15).IsRequired();
            builder.Property(x => x.CreationDate).HasColumnType("date").HasDefaultValueSql("getDate()").IsRequired();
            builder.Property(x => x.Status).HasMaxLength(15).HasColumnType("varchar").IsRequired();

            builder.HasOne(x => x.Company).WithOne(c => c.Account).HasForeignKey<Company>(c => c.AccountId).IsRequired(false);
            builder.HasOne(x => x.Member).WithOne(m => m.Account).HasForeignKey<Member>(m => m.AccountId).IsRequired(false);
        }
    }
}
