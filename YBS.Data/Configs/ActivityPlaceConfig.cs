using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS.Data.Models;

namespace YBS.Data.Configs
{
    public class ActivityPlaceConfig : IEntityTypeConfiguration<ActivityPlace>
    {
        public void Configure(EntityTypeBuilder<ActivityPlace> builder)
        {
            builder.ToTable("ActivityPlace");
            builder.HasKey(activityPlace => activityPlace.Id);
            builder.Property(activityPlace => activityPlace.Id).ValueGeneratedOnAdd();
            builder.HasOne(activityPlace => activityPlace.Activity).WithMany(activity => activity.ActivityPlaces)
                                                                    .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(activityPlace => activityPlace.FromDock).WithMany(fromDock => fromDock.ActivityPlacesFrom)
                                                                    .HasForeignKey(activityPlace => activityPlace.FromDockId)
                                                                    .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(activityPlace => activityPlace.ToDock).WithMany(fromDock => fromDock.ActivityPlacesTo)
                                                                .HasForeignKey(activityPlace => activityPlace.ToDockId)
                                                                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}