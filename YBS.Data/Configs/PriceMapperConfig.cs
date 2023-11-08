using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS.Data.Models;

namespace YBS.Data.Configs
{
    public class PriceMapperConfig : IEntityTypeConfiguration<PriceMapper>
    {
        public void Configure(EntityTypeBuilder<PriceMapper> builder)
        {
            builder.ToTable("PriceMapper");
            builder.HasKey(priceMapper => priceMapper.Id);
            builder.Property(priceMapper => priceMapper.Id).ValueGeneratedOnAdd();
            builder.HasOne(priceMapper => priceMapper.YachtType).WithMany(yachtType => yachtType.PriceMappers)
                                                                .HasForeignKey(yachtType => yachtType.YachtTypeId)
                                                                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(priceMapper => priceMapper.Route).WithMany(route => route.PriceMappers)
                                                            .HasForeignKey(priceMapper => priceMapper.RouteId);
            builder.Property(priceMapper => priceMapper.MoneyUnit).HasColumnType("varchar(10)");
        }
    }
}