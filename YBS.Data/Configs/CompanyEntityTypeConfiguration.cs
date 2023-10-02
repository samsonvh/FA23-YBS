using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS.Data.Extensions.Enums;
using YBS.Data.Models;


namespace YBS.Data.Configs
{
    public class CompanyEntityTypeConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {

            builder.ToTable("Company");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Address).HasMaxLength(150).IsRequired();
            builder.Property(x => x.HotLine).HasMaxLength(15).HasColumnType("varchar").IsRequired();
            builder.HasIndex(x => x.HotLine).IsUnique();
            builder.Property(x => x.Logo).HasMaxLength(255).HasColumnType("varchar").IsRequired();
            builder.Property(x => x.FacebookUrl).HasMaxLength(255).HasColumnType("varchar");
            builder.Property(x => x.InstagramURL).HasMaxLength(255).HasColumnType("varchar");
            builder.Property(x => x.LinkedInURL).HasMaxLength(255).HasColumnType("varchar");
            builder.Property(x => x.ContractStartDate).HasColumnType("date").IsRequired();
            builder.Property(x => x.Status).HasColumnType("varchar").HasMaxLength(15).IsRequired()
            .HasConversion(
                x => x.ToString(),
                x => (CompanyStatus)Enum.Parse(typeof(CompanyStatus), x)
            );

        }
    }
}
