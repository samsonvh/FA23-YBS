using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS.Data.Models;

namespace YBS.Data.Configs
{
    public class ServicePackageItemConfig : IEntityTypeConfiguration<ServicePackageItem>
    {
        public void Configure(EntityTypeBuilder<ServicePackageItem> builder)
        {
            builder.ToTable("ServicePackageItem");
            builder.HasKey(servicePackageItem => servicePackageItem.Id);
            builder.Property(servicePackageItem => servicePackageItem.Id).ValueGeneratedOnAdd();
            builder.HasOne(servicePackageItem => servicePackageItem.Service).WithMany(service => service.ServicePackageItems)
                                                                            .HasForeignKey(servicePackageItem => servicePackageItem.ServiceId)
                                                                            .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(servicePackageItem => servicePackageItem.ServicePackage).WithMany(servicePackage => servicePackage.ServicePackageItems)
                                                                                    .HasForeignKey(servicePackageItem => servicePackageItem.ServicePackageId)
                                                                                    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}