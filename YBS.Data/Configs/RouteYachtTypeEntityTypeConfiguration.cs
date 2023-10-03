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
    public class RouteYachtTypeEntityTypeConfiguration : IEntityTypeConfiguration<RouteYachtType>
    {
        public void Configure(EntityTypeBuilder<RouteYachtType> builder)
        {
            builder.ToTable("RouteYachtType");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Price).HasColumnType("float").IsRequired();
            builder.Property(x => x.Unit).HasColumnType("varchar").HasMaxLength(10).IsRequired();
        }
    }
}
