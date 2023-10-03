using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Models;

namespace YBS.Data.Configs
{
    public class DockActivityEntityTypeConfiguration : IEntityTypeConfiguration<DockActivity>
    {
        public void Configure(EntityTypeBuilder<DockActivity> builder)
        {
            builder.ToTable("DockActivity");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
