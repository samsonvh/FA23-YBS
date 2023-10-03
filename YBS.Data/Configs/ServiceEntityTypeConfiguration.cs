using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Models;

namespace YBS.Data.Configs
{
    public class ServiceEntityTypeConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.ToTable("Service");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).HasColumnType("varchar").HasMaxLength(50).IsRequired();
            builder.Property(x => x.Type).HasColumnType("varchar").HasMaxLength(50).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(255).IsRequired(false);
            builder.Property(x => x.Price).HasColumnType("float").IsRequired();
            builder.Property(x => x.Unit).HasColumnType("varchar").HasMaxLength(10).IsRequired();

        }
    }
}
