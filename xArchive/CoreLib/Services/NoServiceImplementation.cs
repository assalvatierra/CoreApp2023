using CoreLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using JobsV1.Models;

namespace CoreLib.Services
{
    internal class NoServiceImplementation
    {
    }


    public class No_SupplierServices : ISupplierService, ISupplierRefs
    {
        Supplier ISupplierService.GetSupplier(int Id)
        {
            throw new NotImplementedException();
        }

        IQueryable<Supplier> ISupplierService.GetSuppliers()
        {
            throw new NotImplementedException();
        }
        public int UpdateSupplier(JobsV1.Models.Supplier supplier)
        {
            throw new NotImplementedException();
        }

        public IQueryable<SupplierType> getSupplierTypes()
        {
            //throw new NotImplementedException(); //breaks the code
            return new List<SupplierType>().AsQueryable();
        }

    }




}
