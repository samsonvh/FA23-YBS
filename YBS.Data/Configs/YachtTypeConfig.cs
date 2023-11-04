using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS.Data.Models;

namespace YBS.Data.Configs
{
    public class YachtTypeConfig : IEntityTypeConfiguration<YachtType>
    {
        public void Configure(EntityTypeBuilder<YachtType> builder)
        {
            builder.ToTable("YachtType");
            builder.HasKey(yachtType => yachtType.Id);
            builder.Property(yachtType => yachtType.Id).ValueGeneratedOnAdd();
            builder.Property(yachtType => yachtType.Name).HasColumnType("nvarchar(100)");
            builder.Property(yachtType => yachtType.Description).HasColumnType("nvarchar(max)").IsRequired(false);
        }
    }
}