using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.DTO.Suppliers
{
    public class cSupplierItems
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public IEnumerable<string> Product { get; set; }
        public IEnumerable<string> ContactPerson { get; set; }
        public IEnumerable<string> ContactNumber { get; set; }
    }
}
