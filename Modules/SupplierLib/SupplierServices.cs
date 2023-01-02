using System.Threading.Tasks;
using CoreLib.Interfaces;
using CoreLib.Models;
using JobsV1.Models;
using Microsoft.EntityFrameworkCore;


namespace SupplierLib
{
    public class SupplierServices : ISupplierService
    {
        private CoreDBContext _context;
        public SupplierServices(CoreDBContext context)
        { 
            this._context = context;

        }

        Supplier ISupplierService.GetSupplier(int Id)
        {
            throw new NotImplementedException();
        }

        async Task<IQueryable<Supplier>> ISupplierService.GetSuppliers()
        {
            return _context.Suppliers
               .Include(s => s.City)
               .Include(s => s.Country)
               .Include(s => s.SupplierType)
               .Where(d => d.Status == "INC");
        }
    }
}