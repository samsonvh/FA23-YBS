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
    public class YachtTypeEntityTypeConfiguration : IEntityTypeConfiguration<YachtType>
    {
        public void Configure(EntityTypeBuilder<YachtType> builder)
        {
            builder.ToTable("YachtType");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).HasColumnType("varchar").HasMaxLength(50).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(255).IsRequired();

            builder.HasMany(x => x.DockYachtTypes)
                .WithOne(y => y.YachtType)
                .HasForeignKey(y => y.YachtTypeId);

            builder.HasMany(x => x.RouteYachtTypes)
                .WithOne(y => y.YachtType)
                .HasForeignKey(y => y.YachtTypeId);
        }
    }
}
