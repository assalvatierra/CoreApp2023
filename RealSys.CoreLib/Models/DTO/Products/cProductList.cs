using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.DTO.Products
{
    public class cProductList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SupplierId { get; set; }
        public string Supplier { get; set; }
        public string ItemRate { get; set; }
        public string Unit { get; set; }
        public string DtEntered { get; set; }
        public string DtValidFrom { get; set; }
        public string DtValidTo { get; set; }
        public string Remarks { get; set; }
        public string Particulars { get; set; }
        public int IsValid { get; set; }
        public string Origin { get; set; }
        public string Material { get; set; }
    }
}
