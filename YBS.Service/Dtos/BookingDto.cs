using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;
using YBS.Data.Models;


namespace YBS.Service.Dtos
{
    public class BookingDto
    {
        public int Id { get; set; }
        public DateTime ActualPickedUpTime { get; set; }
        public DateTime ActualStartingDate { get; set; }
        public DateTime ActualEndingDate { get; set; }
        public int ActualDurationTime { get; set; }
        public string DurationUnit { get; set; }
        public string? Note { get; set; }
        public float TotalPrice { get; set; }
        public string MoneyUnit { get; set; }
        public string Status { get; set; }
    }
}


