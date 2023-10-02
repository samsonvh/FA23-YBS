﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YBS.Services.Request
{
    public class PageRequest
    {
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public string? OrderBy { get; set; }
        public string? Direction { get; set; }
    }
}