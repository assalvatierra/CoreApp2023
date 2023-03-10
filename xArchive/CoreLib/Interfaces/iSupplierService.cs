using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobsV1.Models;

namespace CoreLib.Interfaces
{
    public interface ISupplierService
    {
        public IQueryable<JobsV1.Models.Supplier> GetSuppliers();
        public JobsV1.Models.Supplier GetSupplier(int Id);
        public int UpdateSupplier(JobsV1.Models.Supplier supplier);


    }
}
