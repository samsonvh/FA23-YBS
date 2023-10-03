using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS.Data.Enums;
using YBS.Data.Models;

namespace YBS.Data.Configs
{
    public class DockEntityTypeConfiguration : IEntityTypeConfiguration<Dock>
    {
        public void Configure(EntityTypeBuilder<Dock> builder)
        {

            builder.ToTable("Dock");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Address).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Lattiude).IsRequired();
            builder.Property(x => x.Longtiude).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(255).IsRequired(false);
            builder.Property(x => x.LastModifiedDate).HasColumnType("date").IsRequired();
            builder.Property(x => x.Status).HasMaxLength(15).IsRequired();

            builder.HasMany(x => x.Activities)
                .WithOne(a => a.StartDock)
                .HasForeignKey(a => a.StartDockId)
                .IsRequired(false);
        }
    }
}