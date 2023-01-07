using CoreLib.Interfaces;
using JobsV1.Models;

using Microsoft.EntityFrameworkCore;

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


    }
}

