﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Service.Dtos.PageRequests;

namespace YBS.Service.Dtos.ListingDtos
{
    public class RouteListingDto
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Beginning { get; set; }
        public string Destination { get; set; }
        public string Status { get; set; }

    }
}