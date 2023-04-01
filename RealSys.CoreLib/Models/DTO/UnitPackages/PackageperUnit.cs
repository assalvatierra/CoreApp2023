using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.DTO.UnitPackages
{
    public class PackageperUnit
    {
        public int Id { get; set; }
        public string PkgDesc { get; set; }
        public decimal RateperDay { get; set; }
        public decimal RateperWeek { get; set; }
        public decimal RateperMonth { get; set; }
        public decimal AddOn { get; set; }
        public decimal FuelLonghaul { get; set; }
        public decimal FuelDaily { get; set; }
        public decimal Meals { get; set; }
        public decimal Acc { get; set; }
        public string Unit { get; set; }
        public string Group { get; set; }
        public string Status { get; set; }
    }
}
