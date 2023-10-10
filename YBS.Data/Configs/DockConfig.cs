using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS.Data.Models;

namespace YBS.Data.Configs
{
    public class DockConfig : IEntityTypeConfiguration<Dock>
    {
        public void Configure(EntityTypeBuilder<Dock> builder)
        {
            builder.ToTable("Dock");
            builder.HasKey(dock => dock.Id);
            builder.HasOne(dock => dock.Company).WithMany(company => company.Docks).HasForeignKey(dock => dock.CompanyId);
            builder.Property(dock => dock.Name).HasColumnType("nvarchar(100)");
            builder.Property(dock => dock.Address).HasColumnType("nvarchar(100)");
            builder.Property(dock => dock.Latitude).HasColumnType("float");
            builder.Property(dock => dock.Longtitude).HasColumnType("float");
            builder.Property(dock => dock.Description).HasColumnType("nvarchar(100)").IsRequired(false);
            builder.Property(dock => dock.CreationDate).HasColumnType("datetime").HasDefaultValueSql("getDate()");
            builder.Property(dock => dock.LastModifiedDate).HasColumnType("datetime").HasDefaultValueSql("getDate()");
        }
    }
}