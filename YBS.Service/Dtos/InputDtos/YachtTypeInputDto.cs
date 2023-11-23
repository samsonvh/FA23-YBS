using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YBS.Service.Dtos.InputDtos
{
    public class YachtTypeInputDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
