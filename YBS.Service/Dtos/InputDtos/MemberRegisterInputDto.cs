using Microsoft.AspNetCore.Http;
using YBS.Data.Enums;
using YBS.Data.Models;

namespace YBS.Service.Dtos.InputDtos
{
    public class MemberRegisterInputDto
    {
        public int MembershipPackageId { get; set; }
        //Account field
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        //Member field
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Nationality { get; set; }
        public EnumGender Gender { get; set; }
        public string Address { get; set; }        
        public string IdentityNumber { get; set; }
        //transaction 
        public string TransactionName { get; set; }
        public EnumTransactionType TransactionType { get; set; }
        public EnumPaymentMethod PaymentMethod { get; set; }
        public float Amount { get; set; }
        public string MoneyUnit { get; set; }
        //vnpay return field
        public string TransactionStatus { get; set; }
        public string ResponseCode { get; set; }
    }
}