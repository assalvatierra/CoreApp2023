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
    public class SalesLeadsDxController : Controller
    {
        private ErpDbContext _context;

        public SalesLeadsDxController(ErpDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            var salesleads = _context.SalesLeads.Select(i => new
            {
                i.Id,
                i.Date,
                i.Details,
                i.Remarks,
                i.CustomerId,
                i.CustName,
                i.DtEntered,
                i.EnteredBy,
                i.Price,
                i.AssignedTo,
                i.CustPhone,
                i.CustEmail,
                i.SalesCode,
                i.ItemWeight,
                i.Commodity
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "Id" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(salesleads, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values)
        {
            var model = new SalesLead();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if (!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.SalesLeads.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.Id });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values)
        {
            var model = await _context.SalesLeads.FirstOrDefaultAsync(item => item.Id == key);
            if (model == null)
                return StatusCode(409, "Object not found");

            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if (!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task Delete(int key)
        {
            var model = await _context.SalesLeads.FirstOrDefaultAsync(item => item.Id == key);

            _context.SalesLeads.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> CustomersLookup(DataSourceLoadOptions loadOptions)
        {
            var lookup = from i in _context.Customers
                         orderby i.Name
                         select new
                         {
                             Value = i.Id,
                             Text = i.Name
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(SalesLead model, IDictionary values)
        {
            string ID = nameof(SalesLead.Id);
            string DATE = nameof(SalesLead.Date);
            string DETAILS = nameof(SalesLead.Details);
            string REMARKS = nameof(SalesLead.Remarks);
            string CUSTOMER_ID = nameof(SalesLead.CustomerId);
            string CUST_NAME = nameof(SalesLead.CustName);
            string DT_ENTERED = nameof(SalesLead.DtEntered);
            string ENTERED_BY = nameof(SalesLead.EnteredBy);
            string PRICE = nameof(SalesLead.Price);
            string ASSIGNED_TO = nameof(SalesLead.AssignedTo);
            string CUST_PHONE = nameof(SalesLead.CustPhone);
            string CUST_EMAIL = nameof(SalesLead.CustEmail);
            string SALES_CODE = nameof(SalesLead.SalesCode);
            string ITEM_WEIGHT = nameof(SalesLead.ItemWeight);
            string COMMODITY = nameof(SalesLead.Commodity);

            if (values.Contains(ID))
            {
                model.Id = Convert.ToInt32(values[ID]);
            }

            if (values.Contains(DATE))
            {
                model.Date = Convert.ToDateTime(values[DATE]);
            }

            if (values.Contains(DETAILS))
            {
                model.Details = Convert.ToString(values[DETAILS]);
            }

            if (values.Contains(REMARKS))
            {
                model.Remarks = Convert.ToString(values[REMARKS]);
            }

            if (values.Contains(CUSTOMER_ID))
            {
                model.CustomerId = Convert.ToInt32(values[CUSTOMER_ID]);
            }

            if (values.Contains(CUST_NAME))
            {
                model.CustName = Convert.ToString(values[CUST_NAME]);
            }

            if (values.Contains(DT_ENTERED))
            {
                model.DtEntered = Convert.ToDateTime(values[DT_ENTERED]);
            }

            if (values.Contains(ENTERED_BY))
            {
                model.EnteredBy = Convert.ToString(values[ENTERED_BY]);
            }

            if (values.Contains(PRICE))
            {
                model.Price = Convert.ToDecimal(values[PRICE], CultureInfo.InvariantCulture);
            }

            if (values.Contains(ASSIGNED_TO))
            {
                model.AssignedTo = Convert.ToString(values[ASSIGNED_TO]);
            }

            if (values.Contains(CUST_PHONE))
            {
                model.CustPhone = Convert.ToString(values[CUST_PHONE]);
            }

            if (values.Contains(CUST_EMAIL))
            {
                model.CustEmail = Convert.ToString(values[CUST_EMAIL]);
            }

            if (values.Contains(SALES_CODE))
            {
                model.SalesCode = Convert.ToString(values[SALES_CODE]);
            }

            if (values.Contains(ITEM_WEIGHT))
            {
                model.ItemWeight = Convert.ToString(values[ITEM_WEIGHT]);
            }

            if (values.Contains(COMMODITY))
            {
                model.Commodity = Convert.ToString(values[COMMODITY]);
            }
        }

        private string GetFullErrorMessage(ModelStateDictionary modelState)
        {
            var messages = new List<string>();

            foreach (var entry in modelState)
            {
                foreach (var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return string.Join(" ", messages);
        }
    }
}