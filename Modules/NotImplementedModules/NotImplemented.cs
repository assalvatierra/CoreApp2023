using CoreLib.Interfaces;
using JobsV1.Models;

namespace NotImplementedModules
{
    public class SupplierServices : ISupplierService
    {
        Supplier ISupplierService.GetSupplier(int Id)
        {
            throw new NotImplementedException();
        }

        IQueryable<Supplier> ISupplierService.GetSuppliers()
        {
            throw new NotImplementedException();
        }
    }
}