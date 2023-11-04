using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace YBS.Data.Models
{
    public class Facility
    {
        public int Id { get; set; }
        public int YachtId { get; set; }
        [ForeignKey("YachtId")]
        public Yacht Yacht { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}