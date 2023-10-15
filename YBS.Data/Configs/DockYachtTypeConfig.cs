using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS.Data.Models;

namespace YBS.Data.Configs
{
    public class DockYachtTypeConfig : IEntityTypeConfiguration<DockYachtType>
    {
        public void Configure(EntityTypeBuilder<DockYachtType> builder)
        {
            builder.ToTable("DockYachtType");
            builder.HasKey(dockyachtType => dockyachtType.Id);
            builder.Property(dockyachtType => dockyachtType.Id).ValueGeneratedOnAdd();
        }
    }
}