﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YBS.Data.Enums;
using YBS.Data.Models;

namespace YBS.Service.Dtos
{
    public class YachtDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public string Description { get; set; }
        public string Manufacture { get; set; }
        public int GrossTonnage { get; set; }
        public string GrossTonnageUnit { get; set; }
        public int Range { get; set; }
        public string RangeUnit { get; set; }
        public int TotalCrew { get; set; }
        public int CrusingSpeed { get; set; }
        public int MaxSpeed { get; set; }
        public string SpeedUnit { get; set; }
        public int Year { get; set; }
        public float LOA { get; set; }
        public float BEAM { get; set; }
        public float DRAFT { get; set; }
        public string SizeUnit { get; set; }
        public int FuelCapacity { get; set; }
        public string FuelCapacityUnit { get; set; }
        public int MaximumGuestLimit { get; set; }
        public int Cabin { get; set; }
        public DateTime CreationDate { get; set; }
        public EnumYachtStatus Status { get; set; }
    }
}
