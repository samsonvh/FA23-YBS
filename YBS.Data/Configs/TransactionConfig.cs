using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YBS.Data.Models;

namespace YBS.Data.Configs
{
    public class TransactionConfig : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transaction");
            builder.HasKey(transaction => transaction.Id);
            builder.Property(transaction => transaction.Id).ValueGeneratedOnAdd();
            builder.Property(transaction => transaction.Code).HasColumnType("nvarchar(20)");
            builder.Property(transaction => transaction.MoneyUnit).HasColumnType("varchar(15)");
            builder.Property(transaction => transaction.CreationDate).HasColumnType("datetime");    
            builder.Property(transaction => transaction.Amount).HasColumnType("float");    
        }
    }
}