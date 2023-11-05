using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS.Data.Models;

namespace YBS.Data.Configs
{
    public class RouteServicePackageConfig : IEntityTypeConfiguration<RouteServicePackage>
    {
        public void Configure(EntityTypeBuilder<RouteServicePackage> builder)
        {
            builder.ToTable("RouteServicePackage");
            builder.HasKey(routeServicePackage => routeServicePackage.Id);
            builder.Property(routeServicePackage => routeServicePackage.Id).ValueGeneratedOnAdd();
            builder.HasOne(routeServicePackage => routeServicePackage.Route).WithMany(route => route.RouteServicePackages)
                                                                            .HasForeignKey(routeServicePackage => routeServicePackage.RouteId)
                                                                            .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(routeServicePackage => routeServicePackage.ServicePackage).WithMany(servicePackage => servicePackage.RouteServicePackages)
                                                                                    .HasForeignKey(routeServicePackage => routeServicePackage.ServicePackageId)
                                                                                    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}