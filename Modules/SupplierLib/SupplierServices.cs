using CoreLib.Interfaces;
using CoreLib.Models;
using JobsV1.Models;
using Microsoft.EntityFrameworkCore;


namespace SupplierLib
{
    public class SupplierServices : iSupplierService
    {
        private CoreDBContext _context;
        public SupplierServices(CoreDBContext context)
        { 
            this._context = context;

        }

        Supplier iSupplierService.GetSupplier(int Id)
        {
            throw new NotImplementedException();
        }

        IQueryable<Supplier> iSupplierService.GetSuppliers()
        {
            return _context.Suppliers
               .Include(s => s.City)
               .Include(s => s.Country)
               .Include(s => s.SupplierType)
               .Where(d => d.Status == "INC");
        }
    }
}