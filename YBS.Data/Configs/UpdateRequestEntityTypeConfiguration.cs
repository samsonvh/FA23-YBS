using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS.Data.Models;

namespace YBS.Data.Configs
{
    public class UpdateRequestEntityTypeConfiguration : IEntityTypeConfiguration<UpdateRequest>
    {
        public void Configure(EntityTypeBuilder<UpdateRequest> builder)
        {
            builder.ToTable("UpdateRequest");
            builder.HasKey(updateRequest => updateRequest.Id);

            builder.Property(updateRequest => updateRequest.Id).ValueGeneratedOnAdd();
            builder.Property(updateRequest => updateRequest.Name).HasMaxLength(100).IsRequired();
            builder.Property(updateRequest => updateRequest.Address).HasMaxLength(150).IsRequired();
            builder.Property(updateRequest => updateRequest.HotLine).HasMaxLength(15).HasColumnType("varchar").IsRequired();
            builder.Property(updateRequest => updateRequest.Logo).HasMaxLength(255).HasColumnType("varchar").IsRequired();
            builder.Property(updateRequest => updateRequest.FacebookUrl).HasMaxLength(255).HasColumnType("varchar").IsRequired(false);
            builder.Property(updateRequest => updateRequest.InstagramUrl).HasMaxLength(255).HasColumnType("varchar").IsRequired(false);
            builder.Property(updateRequest => updateRequest.LinkedInUrl).HasMaxLength(255).HasColumnType("varchar").IsRequired(false);
            builder.Property(updateRequest => updateRequest.ContractStartDate).HasColumnType("date").IsRequired();
            builder.Property(updateRequest => updateRequest.LastModifiedDate).HasColumnType("date").HasDefaultValueSql("getDate()").IsRequired();
            builder.Property(updateRequest => updateRequest.Status).IsRequired();
        }

    }
}