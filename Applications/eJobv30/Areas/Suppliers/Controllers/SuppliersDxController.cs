using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using RealSys.CoreLib.Models.Erp;

namespace eJobv30.Controllers
{
    [Route("api/[controller]/[action]")]
    public class SuppliersDxController : Controller
    {
        private ErpDbContext _context;

        public SuppliersDxController(ErpDbContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var suppliers = _context.Suppliers.Select(i => new {
                i.Id,
                i.Name,
                i.Contact1,
                i.Contact2,
                i.Contact3,
                i.Email,
                i.Details,
                i.CityId,
                i.City,
                i.SupplierTypeId,
                i.SupplierType,
                i.Status,
                i.Website,
                i.Address,
                i.CountryId,
                i.Country,
                i.Code,
                i.SupplierContacts
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "Id" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(suppliers, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Supplier();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Suppliers.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.Id });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Suppliers.FirstOrDefaultAsync(item => item.Id == key);
            if(model == null)
                return StatusCode(409, "Object not found");

            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task Delete(int key) {
            var model = await _context.Suppliers.FirstOrDefaultAsync(item => item.Id == key);

            _context.Suppliers.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> CitiesLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Cities
                         orderby i.Name
                         select new {
                             Value = i.Id,
                             Text = i.Name
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> SupplierTypesLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.SupplierTypes
                         orderby i.Description
                         select new {
                             Value = i.Id,
                             Text = i.Description
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> CountriesLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Countries
                         orderby i.Name
                         select new {
                             Value = i.Id,
                             Text = i.Name
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(Supplier model, IDictionary values) {
            string ID = nameof(Supplier.Id);
            string NAME = nameof(Supplier.Name);
            string CONTACT1 = nameof(Supplier.Contact1);
            string CONTACT2 = nameof(Supplier.Contact2);
            string CONTACT3 = nameof(Supplier.Contact3);
            string EMAIL = nameof(Supplier.Email);
            string DETAILS = nameof(Supplier.Details);
            string CITY_ID = nameof(Supplier.CityId);
            string SUPPLIER_TYPE_ID = nameof(Supplier.SupplierTypeId);
            string STATUS = nameof(Supplier.Status);
            string WEBSITE = nameof(Supplier.Website);
            string ADDRESS = nameof(Supplier.Address);
            string COUNTRY_ID = nameof(Supplier.CountryId);
            string CODE = nameof(Supplier.Code);

            if(values.Contains(ID)) {
                model.Id = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(NAME)) {
                model.Name = Convert.ToString(values[NAME]);
            }

            if(values.Contains(CONTACT1)) {
                model.Contact1 = Convert.ToString(values[CONTACT1]);
            }

            if(values.Contains(CONTACT2)) {
                model.Contact2 = Convert.ToString(values[CONTACT2]);
            }

            if(values.Contains(CONTACT3)) {
                model.Contact3 = Convert.ToString(values[CONTACT3]);
            }

            if(values.Contains(EMAIL)) {
                model.Email = Convert.ToString(values[EMAIL]);
            }

            if(values.Contains(DETAILS)) {
                model.Details = Convert.ToString(values[DETAILS]);
            }

            if(values.Contains(CITY_ID)) {
                model.CityId = Convert.ToInt32(values[CITY_ID]);
            }

            if(values.Contains(SUPPLIER_TYPE_ID)) {
                model.SupplierTypeId = Convert.ToInt32(values[SUPPLIER_TYPE_ID]);
            }

            if(values.Contains(STATUS)) {
                model.Status = Convert.ToString(values[STATUS]);
            }

            if(values.Contains(WEBSITE)) {
                model.Website = Convert.ToString(values[WEBSITE]);
            }

            if(values.Contains(ADDRESS)) {
                model.Address = Convert.ToString(values[ADDRESS]);
            }

            if(values.Contains(COUNTRY_ID)) {
                model.CountryId = Convert.ToInt32(values[COUNTRY_ID]);
            }

            if(values.Contains(CODE)) {
                model.Code = Convert.ToString(values[CODE]);
            }
        }

        private string GetFullErrorMessage(ModelStateDictionary modelState) {
            var messages = new List<string>();

            foreach(var entry in modelState) {
                foreach(var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join(" ", messages);
        }
    }
}