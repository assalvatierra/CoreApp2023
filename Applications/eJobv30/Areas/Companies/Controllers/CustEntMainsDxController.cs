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
    public class CustEntMainsDxController : Controller
    {
        private ErpDbContext _context;

        public CustEntMainsDxController(ErpDbContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var custentmains = _context.CustEntMains.Select(i => new {
                i.Id,
                i.Name,
                i.Address,
                i.Contact1,
                i.Contact2,
                i.iconPath,
                i.Website,
                i.Remarks,
                i.CityId,
                i.Status,
                i.AssignedTo,
                i.Mobile,
                i.Code,
                i.Exclusive,
                i.CustEntAccountTypeId,
                i.CustEntAccountType,
                i.CustEntCats
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "Id" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(custentmains, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new CustEntMain();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.CustEntMains.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.Id });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.CustEntMains.FirstOrDefaultAsync(item => item.Id == key);
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
            var model = await _context.CustEntMains.FirstOrDefaultAsync(item => item.Id == key);

            _context.CustEntMains.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> CustEntAccountTypesLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.CustEntAccountTypes
                         orderby i.Name
                         select new {
                             Value = i.Id,
                             Text = i.Name
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(CustEntMain model, IDictionary values) {
            string ID = nameof(CustEntMain.Id);
            string NAME = nameof(CustEntMain.Name);
            string ADDRESS = nameof(CustEntMain.Address);
            string CONTACT1 = nameof(CustEntMain.Contact1);
            string CONTACT2 = nameof(CustEntMain.Contact2);
            string ICON_PATH = nameof(CustEntMain.iconPath);
            string WEBSITE = nameof(CustEntMain.Website);
            string REMARKS = nameof(CustEntMain.Remarks);
            string CITY_ID = nameof(CustEntMain.CityId);
            string STATUS = nameof(CustEntMain.Status);
            string ASSIGNED_TO = nameof(CustEntMain.AssignedTo);
            string MOBILE = nameof(CustEntMain.Mobile);
            string CODE = nameof(CustEntMain.Code);
            string EXCLUSIVE = nameof(CustEntMain.Exclusive);
            string CUST_ENT_ACCOUNT_TYPE_ID = nameof(CustEntMain.CustEntAccountTypeId);

            if(values.Contains(ID)) {
                model.Id = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(NAME)) {
                model.Name = Convert.ToString(values[NAME]);
            }

            if(values.Contains(ADDRESS)) {
                model.Address = Convert.ToString(values[ADDRESS]);
            }

            if(values.Contains(CONTACT1)) {
                model.Contact1 = Convert.ToString(values[CONTACT1]);
            }

            if(values.Contains(CONTACT2)) {
                model.Contact2 = Convert.ToString(values[CONTACT2]);
            }

            if(values.Contains(ICON_PATH)) {
                model.iconPath = Convert.ToString(values[ICON_PATH]);
            }

            if(values.Contains(WEBSITE)) {
                model.Website = Convert.ToString(values[WEBSITE]);
            }

            if(values.Contains(REMARKS)) {
                model.Remarks = Convert.ToString(values[REMARKS]);
            }

            if(values.Contains(CITY_ID)) {
                model.CityId = values[CITY_ID] != null ? Convert.ToInt32(values[CITY_ID]) : (int?)null;
            }

            if(values.Contains(STATUS)) {
                model.Status = Convert.ToString(values[STATUS]);
            }

            if(values.Contains(ASSIGNED_TO)) {
                model.AssignedTo = Convert.ToString(values[ASSIGNED_TO]);
            }

            if(values.Contains(MOBILE)) {
                model.Mobile = Convert.ToString(values[MOBILE]);
            }

            if(values.Contains(CODE)) {
                model.Code = Convert.ToString(values[CODE]);
            }

            if(values.Contains(EXCLUSIVE)) {
                model.Exclusive = Convert.ToString(values[EXCLUSIVE]);
            }

            if(values.Contains(CUST_ENT_ACCOUNT_TYPE_ID)) {
                model.CustEntAccountTypeId = Convert.ToInt32(values[CUST_ENT_ACCOUNT_TYPE_ID]);
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