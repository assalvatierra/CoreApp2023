using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.DTO.Suppliers
{
    public class cSupplierList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Contact1 { get; set; }
        public string Contact2 { get; set; }
        public string Contact3 { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string SupType { get; set; }
        public string City { get; set; }
        public string Details { get; set; }
        public string Country { get; set; }
        public int CountryId { get; set; }
        public string Code { get; set; }
    }
}
