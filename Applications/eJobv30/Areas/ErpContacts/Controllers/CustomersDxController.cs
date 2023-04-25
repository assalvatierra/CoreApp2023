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
    public class CustomersDxController : Controller
    {
        private ErpDbContext _context;

        public CustomersDxController(ErpDbContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var customers = _context.Customers.Select(i => new {
                i.Id,
                i.Name,
                i.Email,
                i.Contact1,
                i.Contact2,
                i.Remarks,
                i.Status
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "Id" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(customers, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Customer();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Customers.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.Id });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Customers.FirstOrDefaultAsync(item => item.Id == key);
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
            var model = await _context.Customers.FirstOrDefaultAsync(item => item.Id == key);

            _context.Customers.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(Customer model, IDictionary values) {
            string ID = nameof(Customer.Id);
            string NAME = nameof(Customer.Name);
            string EMAIL = nameof(Customer.Email);
            string CONTACT1 = nameof(Customer.Contact1);
            string CONTACT2 = nameof(Customer.Contact2);
            string REMARKS = nameof(Customer.Remarks);
            string STATUS = nameof(Customer.Status);

            if(values.Contains(ID)) {
                model.Id = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(NAME)) {
                model.Name = Convert.ToString(values[NAME]);
            }

            if(values.Contains(EMAIL)) {
                model.Email = Convert.ToString(values[EMAIL]);
            }

            if(values.Contains(CONTACT1)) {
                model.Contact1 = Convert.ToString(values[CONTACT1]);
            }

            if(values.Contains(CONTACT2)) {
                model.Contact2 = Convert.ToString(values[CONTACT2]);
            }

            if(values.Contains(REMARKS)) {
                model.Remarks = Convert.ToString(values[REMARKS]);
            }

            if(values.Contains(STATUS)) {
                model.Status = Convert.ToString(values[STATUS]);
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