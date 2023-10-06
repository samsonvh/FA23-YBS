using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace YBS.Data.Configs
{
    public class CompanyEntityTypeConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Company");
            builder.HasKey(company => company.Id);

            builder.Property(company => company.Id).ValueGeneratedOnAdd();
            builder.Property(company => company.Name).HasMaxLength(100).IsRequired();
            builder.Property(company => company.Address).HasMaxLength(150).IsRequired();
            builder.Property(company => company.HotLine).HasMaxLength(15).HasColumnType("varchar").IsRequired();
            builder.Property(company => company.Logo).HasMaxLength(255).HasColumnType("varchar").IsRequired();
            builder.Property(company => company.FacebookUrl).HasMaxLength(255).HasColumnType("varchar").IsRequired(false);
            builder.Property(company => company.InstagramUrl).HasMaxLength(255).HasColumnType("varchar").IsRequired(false);
            builder.Property(company => company.LinkedInUrl).HasMaxLength(255).HasColumnType("varchar").IsRequired(false);
            builder.Property(company => company.ContractStartDate).HasColumnType("date").IsRequired();
            builder.Property(company => company.LastModifiedDate).HasColumnType("date").HasDefaultValueSql("getDate()").IsRequired();
            builder.Property(company => company.Status).IsRequired();
        }
    }
}
