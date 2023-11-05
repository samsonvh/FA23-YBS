using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS.Data.Models;

namespace YBS.Data.Configs
{
    public class YachtMooringConfig : IEntityTypeConfiguration<YachtMooring>
    {
        public void Configure(EntityTypeBuilder<YachtMooring> builder)
        {
            builder.ToTable("YachtMooring");
            builder.HasKey(yachtMooring => yachtMooring.Id);
            builder.Property(yachtMooring => yachtMooring.Id).ValueGeneratedOnAdd();
            builder.Property(yachtMooring =>yachtMooring.ArrivalTime).HasColumnType("datetime");
            builder.HasOne(yachtMooring => yachtMooring.Yacht).WithMany(yacht => yacht.YachtMoorings)
                                                            .HasForeignKey(yachtMooring => yachtMooring.YachtId)
                                                            .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(yachtMooring => yachtMooring.Dock).WithMany(dock => dock.YachtMoorings)
                                                            .HasForeignKey(yachtMooring => yachtMooring.DockId)
                                                            .OnDelete(DeleteBehavior.NoAction);
        }
    }
}