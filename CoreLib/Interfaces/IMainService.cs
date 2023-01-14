using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobsV1.Models;

namespace CoreLib.Interfaces
{
    public interface IMainService
    {
        public ISupplierService SupplierSvc { get; }

        public Task<IQueryable<Supplier>> GetSuppliers();
        public Supplier GetSupplier(int Id);
        public int UpdateSupplier(JobsV1.Models.Supplier supplier);
    }
}
