﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YBS.Service.Dtos.ListingDtos
{
    public class BookingPaymentListingDto
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public float TotalPrice { get; set; }
        public string Status { get; set; }
    }
}