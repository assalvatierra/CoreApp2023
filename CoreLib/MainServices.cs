using CoreLib.Interfaces;

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



    }
}

