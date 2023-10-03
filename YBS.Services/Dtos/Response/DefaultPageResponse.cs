﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YBS.Services.Dtos.Response
{
    public class DefaultPageResponse<T>
    {
        public List<T>? Data { get; set; }
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
    }
}