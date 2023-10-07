using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS.Data.Models;

namespace YBS.Data.Configs
{
    public class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");
            builder.HasKey(role => role.ID);
            builder.Property(role => role.Name).HasColumnType("varchar(50)");
        }
    }
}