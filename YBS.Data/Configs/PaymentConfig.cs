using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS.Data.Models;

namespace YBS.Data.Configs
{
    public class PaymentMethod : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payment");
            builder.HasKey(payment => payment.Id);
            builder.Property(payment => payment.Id).ValueGeneratedOnAdd();
            builder.Property(payment => payment.Name).HasColumnType("nvarchar(100)");
            builder.Property(payment => payment.MoneyUnit).HasColumnType("varchar(10)");
            builder.Property(payment => payment.PaymentDate).HasColumnType("datetime");
            builder.Property(payment => payment.TotalPrice).HasColumnType("float");
        }
    }
}