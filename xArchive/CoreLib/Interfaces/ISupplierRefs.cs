using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobsV1.Models;

namespace CoreLib.Interfaces
{
    public interface ISupplierRefs
    {
        public IQueryable<SupplierType> getSupplierTypes(); 
    }
}
