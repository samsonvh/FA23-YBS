using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS.Service.Dtos.InputDtos
{
    public class PaymentInformationInputDto
    {
        public int PaymentId { get; set; }
        public string Name { get; set; }
        public float TotalPrice { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}