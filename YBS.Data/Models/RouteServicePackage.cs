using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace YBS.Data.Models
{
    public class RouteServicePackage
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public Route Route { get; set; }
        public int ServicePackageId { get; set; }
        public ServicePackage ServicePackage { get; set; }    
    }
}