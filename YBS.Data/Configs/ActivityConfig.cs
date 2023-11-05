using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS.Data.Models;

namespace YBS.Data.Configs
{
    public class ActivityConfig : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder)
        {
            builder.ToTable("Activity");
            builder.HasKey(activity => activity.Id);
            builder.Property(activity => activity.Id).ValueGeneratedOnAdd();
            builder.Property(activity => activity.Name).HasColumnType("nvarchar(100)");
            builder.Property(activity => activity.Name).HasColumnType("nvarchar(max)").IsRequired(false);
            builder.Property(activity => activity.OccuringTime).HasColumnType("time");
        }
    }
}