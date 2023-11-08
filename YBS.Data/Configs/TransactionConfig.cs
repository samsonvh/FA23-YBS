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
            builder.Property(transaction => transaction.Name).HasColumnType("nvarchar(500)");
            builder.Property(transaction => transaction.MoneyUnit).HasColumnType("varchar(15)");
            builder.Property(transaction => transaction.CreationDate).HasColumnType("datetime");    
            builder.Property(transaction => transaction.Amount).HasColumnType("float");
            builder.Property(transaction => transaction.VNPayTmnCode).HasColumnType("varchar(10)").IsRequired(false);
            builder.Property(transaction => transaction.VNPayTxnRef).HasColumnType("varchar(10)").IsRequired(false);
            builder.Property(transaction => transaction.VNPayResponseCode).HasColumnType("varchar(10)").IsRequired(false);
            builder.Property(transaction => transaction.VNPAYBankCode).HasColumnType("varchar(10)").IsRequired(false);
            builder.Property(transaction => transaction.VNPAYcardType).HasColumnType("varchar(10)").IsRequired(false);
            builder.Property(transaction => transaction.VNPAYTransactionNo).HasColumnType("varchar(10)").IsRequired(false);
            builder.Property(transaction => transaction.VNPayTransactionStatus).HasColumnType("varchar(10)").IsRequired(false);    
        }
    }
}