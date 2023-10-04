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
    public class MembershipPackageEntityTypeConfiguration : IEntityTypeConfiguration<MembershipPackage>
    {
        public void Configure(EntityTypeBuilder<MembershipPackage> builder)
        {
            builder.ToTable("MembershipPackage");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

        }
    }
}
