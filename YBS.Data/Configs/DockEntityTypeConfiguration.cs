using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS.Data.Extensions.Enums;
using YBS.Data.Models;

namespace YBS.Data.Configs
{
    public class DockEntityTypeConfiguration : IEntityTypeConfiguration<Dock>
    {
        public void Configure(EntityTypeBuilder<Dock> builder)
        {

            builder.ToTable("Dock");
            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Address).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Lattiude).IsRequired();
            builder.Property(x => x.Longtiude).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(255).IsRequired(false);
            builder.Property(x => x.Status).HasColumnType("varchar").HasMaxLength(15).IsRequired()
            .HasConversion(
                x => x.ToString(),
                x => (DockStatus)Enum.Parse(typeof(DockStatus), x)
            );
        }
    }
}