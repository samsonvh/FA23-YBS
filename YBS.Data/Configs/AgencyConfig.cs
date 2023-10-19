using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS.Data.Models;

namespace YBS.Data.Configs
{
    public class AgencyConfig : IEntityTypeConfiguration<Agency>
    {
        public void Configure(EntityTypeBuilder<Agency> builder)
        {
            builder.ToTable("Agency");
            builder.HasKey(agency => agency.Id);
            builder.Property(agency => agency.Id).ValueGeneratedOnAdd();
            builder.Property(agency => agency.Name).HasColumnType("nvarchar(100)");
            builder.Property(agency => agency.Address).HasColumnType("nvarchar(100)").IsRequired(false);
            builder.Property(agency => agency.HotLine).HasColumnType("varchar(15)");
        }
    }
}