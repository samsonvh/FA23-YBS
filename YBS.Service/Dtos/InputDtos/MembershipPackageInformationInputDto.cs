using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using YBS.Data.Enums;

namespace YBS.Service.Dtos.InputDtos
{
    public class MembershipPackageInformationInputDto
    {
        public int MembershipPackageId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; } // VNPay ko luu 
        public string Password { get; set; }
        public string? FullName { get; set; }
        public DateTime DateOfBirth { get; set; } //
        public string? PhoneNumber { get; set; }
        public string? Nationality { get; set; }
        public EnumGender Gender { get; set; } //
        public string? Address { get; set; }
        public string? IdentityNumber { get; set; }//
        public string? MoneyUnit { get; set; }
        public EnumTransactionType? TransactionType { get; set; }
        public EnumPaymentMethod? PaymentMethod { get; set; }
    }
}