using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS.Data.Enums;
using YBS.Data.Models;

namespace YBS.Data.Configs
{
    public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.Name).HasMaxLength(10).IsRequired();
            builder.HasIndex(x => x.Name).IsUnique();
            builder.HasMany(x => x.Accounts).WithOne(x => x.Role).HasForeignKey(x => x.RoleID);

            builder.HasMany(x => x.Accounts)
                .WithOne(a => a.Role)
                .HasForeignKey(a => a.RoleID)
                .IsRequired(false);
        }
    }
}