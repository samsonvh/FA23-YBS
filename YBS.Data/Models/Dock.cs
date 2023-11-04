using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using YBS.Data.Enums;

namespace YBS.Data.Models
{
    public class Dock
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public Company Company { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public float Latitude { get; set; }
        public float Longtitude { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public EnumDockStatus Status { get; set; }
        public ICollection<DockYachtType> DockYachtTypes { get; set; }
        public ICollection<YachtMooring>? YachtMoorings { get; set; }
    }
}