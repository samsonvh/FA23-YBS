using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using YBS.Data.Enums;
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

            builder.Property(x => x.Status).HasMaxLength(15).IsRequired();

            //company
            builder.HasOne(x => x.Company)
                .WithOne(c => c.Account)
                .HasForeignKey<Company>(c => c.AccountId)
                .IsRequired(false); //make foreign key optional

            //member
            builder.HasOne(x => x.Member)
                .WithOne(m => m.Account)
                .HasForeignKey<Member>(m => m.AccountId)
                .IsRequired(false); //make foreign key optional
        }
    }
}
