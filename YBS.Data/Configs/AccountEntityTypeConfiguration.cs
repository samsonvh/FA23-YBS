using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Models;

namespace YBS.Data.Configs
{
    public class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Account");
            builder.HasKey(account => account.Id);

            builder.Property(account => account.Id).ValueGeneratedOnAdd();
            builder.Property(account => account.Email).HasMaxLength(200).HasColumnType("varchar").IsRequired();
            builder.HasIndex(account => account.Email).IsUnique();
            builder.Property(account => account.PhoneNumber).HasMaxLength(15).HasColumnType("varchar").IsRequired();
            builder.Property(account => account.CreationDate).HasColumnType("date").HasDefaultValueSql("getDate()").IsRequired();
            builder.Property(account => account.LastModifiedDate).HasColumnType("date").HasDefaultValueSql("getDate()").IsRequired();
            builder.Property(account => account.UserName).HasMaxLength(255).HasColumnType("varchar").IsRequired();
           builder.Property(account => account.HashedPassword).IsRequired();
            builder.Property(account => account.Status).IsRequired();

            builder.HasOne(account => account.Role)
                 .WithMany(account => account.Accounts)
                 .HasForeignKey(account => account.RoleId);

            builder.HasOne(account => account.Company)
                .WithOne(company => company.Account)
                .HasForeignKey<Company>(company => company.AccountId);

            builder.HasOne(account => account.Member)
               .WithOne(member => member.Account)
               .HasForeignKey<Member>(member => member.AccountId);
        }
    }
}
