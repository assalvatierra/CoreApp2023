using CoreLib.Interfaces;
using JobsV1.Models;

using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace CoreLib
{
    public class MainServices: IMainService
    {
        public ISupplierService _supplier;
        public MainServices(ISupplierService supplier) 
        {
            _supplier = supplier;
        }


        public ISupplierService SupplierSvc {
            get { return _supplier; } 
        }

        public async Task<IQueryable<Supplier>> GetSuppliers()
        {
            return _supplier.GetSuppliers();
        }

        public Supplier GetSupplier(int Id)
        {
            return _supplier.GetSupplier(Id);
        }

        public int UpdateSupplier(JobsV1.Models.Supplier supplier)
        {
            return _supplier.UpdateSupplier(supplier);
        }

    }
}

