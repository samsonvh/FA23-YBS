using Microsoft.EntityFrameworkCore;
using YBS.Data.Models;

namespace YBS.Data.Configs
{
    public class CompanyConfig : IEntityTypeConfiguration<Company>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Company");
            builder.HasKey(company => company.Id);
            builder.Property(company => company.Id).ValueGeneratedOnAdd();
            builder.Property(company => company.Name).HasColumnType("nvarchar(100)");
            builder.Property(company => company.Address).HasColumnType("nvarchar(200)");
            builder.Property(company => company.HotLine).HasColumnType("varchar(15)");
            builder.Property(company => company.Logo).HasColumnType("nvarchar(max)");
            builder.Property(company => company.FacebookURL).HasColumnType("nvarchar(max)").IsRequired(false);
            builder.Property(company => company.InstagramURL).HasColumnType("nvarchar(max)").IsRequired(false);
            builder.Property(company => company.LinkedInURL).HasColumnType("nvarchar(max)").IsRequired(false);
            builder.Property(company => company.ContractStartDate).HasColumnType("datetime");
            builder.Property(company => company.LastModifiedDate).HasColumnType("datetime").HasDefaultValueSql("getDate()");
        }
    }
}