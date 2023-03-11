using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLib.Interfaces;
using CoreLib.Models;
using JobsV1.Models;

namespace CoreLib.Services
{
    public class DefaultSharedServices: ISharedService
    {
        private CoreDBContext _context;
        public DefaultSharedServices(CoreDBContext context)
        {
            this._context = context;

        }
        public IQueryable<City> GetCities()
        {
            return _context.Cities;

        }

        public IQueryable<Country> GetCountries()
        {
            return _context.Countries;
        }

    }
}
