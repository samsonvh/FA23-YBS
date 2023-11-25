using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YBS.Service.Dtos.InputDtos
{
    public class DockInputDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public float Latitude { get; set; }
        public float Longtitude { get; set; }
        public string? Description { get; set; }
        public List<IFormFile>? ImageFiles { get; set; }
        public List<int> YachtTypeId { get; set; }
    }
}
