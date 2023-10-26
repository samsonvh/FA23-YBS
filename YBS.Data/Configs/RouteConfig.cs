using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Models;
using Route = YBS.Data.Models.Route;

namespace YBS.Data.Configs
{
    public class RouteConfig : IEntityTypeConfiguration<Route>
    {
        public void Configure(EntityTypeBuilder<Route> builder)
        {
            builder.ToTable("Route");
            builder.HasKey(route => route.Id);
            builder.Property(route => route.Id).ValueGeneratedOnAdd();
            builder.Property(route => route.Name).HasMaxLength(100).IsRequired();
            builder.Property(route => route.Beginning).HasMaxLength(255).IsRequired();
            builder.Property(route => route.Destination).HasMaxLength(255).IsRequired();
            builder.Property(route => route.ImageURL).HasColumnType("nvarchar(max)").IsRequired();
            builder.Property(route => route.ExpectedStartingTime).HasColumnType("time").IsRequired();
            builder.Property(route => route.ExpectedEndingTime).HasColumnType("time").IsRequired();
            builder.Property(route => route.Type).HasColumnType("nvarchar").HasMaxLength(50).IsRequired();
            builder.Property(route => route.Status).IsRequired();
        }
    }
}
