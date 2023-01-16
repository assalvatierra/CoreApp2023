using CoreLib.Interfaces;
using JobsV1.Models;

using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace CoreLib
{
    public class MainServices: IMainService
    {
        public ISharedService _sharedServices;
        public ISupplierService _supplier;
        public ISupplierRefs _supplierrefs;
        public MainServices(
            ISharedService sharedSvc, 
            ISupplierService supplier,
            ISupplierRefs supplierrefs) 
        {
            _supplier = supplier;
            _sharedServices = sharedSvc;
            _supplierrefs = supplierrefs;
        }


        public ISupplierService SupplierSvc {
            get { return _supplier; } 
        }
        public ISharedService SharedSvc
        {
            get { return _sharedServices; }
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

        public ISupplierRefs SupplierRefs {
            get
            {
                return this._supplierrefs;
            }
        }


    }
}

