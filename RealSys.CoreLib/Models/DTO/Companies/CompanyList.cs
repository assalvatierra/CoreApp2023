using RealSys.CoreLib.Models.Erp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.DTO.Companies
{
    public class CompanyList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Website { get; set; }
        public string Remarks { get; set; }
        public string City { get; set; }
        public string Category { get; set; }
        public string AssignedTo { get; set; }
        public string Status { get; set; }
        public string Exclusive { get; set; }

        public IEnumerable<CustEntity> custEntity { get; set; }
    }
}
