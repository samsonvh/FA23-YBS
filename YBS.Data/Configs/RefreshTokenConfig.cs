using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS.Data.Models;
namespace YBS.Data.Configs
{
    public class RefreshTokenConfig : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshToken");
            builder.HasKey(refreshToken => refreshToken.Id);
            builder.Property(refreshToken => refreshToken.Id).ValueGeneratedOnAdd();
            builder.Property(refreshToken => refreshToken.Token).HasColumnType("varchar(100)");
            builder.HasIndex(refreshToken => refreshToken.Token).IsUnique();
            builder.Property(refreshToken => refreshToken.ExpireDate).HasColumnType("datetime");
        }
    }
}