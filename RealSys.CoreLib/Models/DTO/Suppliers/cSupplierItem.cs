using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.DTO.Suppliers
{
    public class cSupplierItem
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public int InvItemId { get; set; }
        public string DtValidFrom { get; set; }
    }
}
