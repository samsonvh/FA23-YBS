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
    public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");
            builder.HasKey(role => role.Id);

            builder.Property(role => role.Id).ValueGeneratedOnAdd();
            builder.Property(role => role.Name).HasMaxLength(10).HasColumnType("varchar").IsRequired();
        }
    }
}
