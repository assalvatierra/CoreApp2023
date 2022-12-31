using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using JobsV1.Models;
using SampleWeb.Data;

namespace Core.Supplier
{
    public class Supplier
    {
        private SampleWebContext _context;
        public Supplier(SampleWebContext context) 
        {
            this._context= context;

        }


        // GET: Suppliers
        public IQueryable<JobsV1.Models.Supplier> GetSuppliers()
        {
            //var sampleWebContext = _context.Suppliers.Include(s => s.City).Include(s => s.Country).Include(s => s.SupplierType);
            //return sampleWebContext.ToList();

            return _context.Suppliers
                .Include(s => s.City)
                .Include(s => s.Country)
                .Include(s => s.SupplierType)
                .Where(d=>d.Status=="INC");
                
        }




    }
}