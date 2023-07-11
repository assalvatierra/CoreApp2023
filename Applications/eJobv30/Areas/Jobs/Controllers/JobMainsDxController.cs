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
using RealSys.Modules.Jobs;
using eJobv30.Areas.Suppliers.Controllers;
using RealSys.Modules.SysLib.Lib;

namespace eJobv30.Controllers
{
    [Route("api/[controller]/[action]")]
    public class JobMainsDxController : Controller
    {
        private ErpDbContext _context;
        private JobOrderClass jobOrderServices;
        private DateClass dt;

        public JobMainsDxController(ErpDbContext context, ILogger<SuppliersController> _logger) {
            _context = context;
            jobOrderServices = new JobOrderClass(_context, _logger);
            dt = new DateClass();
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {

            var today = dt.GetCurrentDate();

            int ONGOING_JOBS = 1;

            //var data = jobOrderServices.GetJobData(ONGOING_JOBS);

            //var jobmains2 = data.Select(i => new {
            //    i.Id,
            //    i.JobDate,
            //    i.Company,
            //    i.Customer,
            //    i.Amount,
            //    i.JobStatus
            //});

            var jobmains = _context.JobMains
                .Where(j=> j.JobDate.Date > today.Date.AddDays(-150) && (j.JobStatusId >= 1 || j.JobStatusId <= 3))
                .Select(i => new {
                i.Id,
                i.JobDate,
                i.CustomerId,
                i.Description,
                i.JobRemarks,
                i.JobStatusId,
                i.AgreedAmt
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "Id" };
            // loadOptions.PaginateViaPrimaryKey = true;

            //return Json(await DataSourceLoader.LoadAsync(jobmains, loadOptions));
            return Json(await DataSourceLoader.LoadAsync(jobmains, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new JobMain();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.JobMains.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.Id });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.JobMains.FirstOrDefaultAsync(item => item.Id == key);
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
            var model = await _context.JobMains.FirstOrDefaultAsync(item => item.Id == key);

            _context.JobMains.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> CustomersLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Customers
                         orderby i.Name
                         select new {
                             Value = i.Id,
                             Text = i.Name
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> BranchesLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Branches
                         orderby i.Name
                         select new {
                             Value = i.Id,
                             Text = i.Name
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> JobStatusLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.JobStatus
                         orderby i.Status
                         select new {
                             Value = i.Id,
                             Text = i.Status
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }


        [HttpGet]
        public async Task<IActionResult> JobAmountsLookup(DataSourceLoadOptions loadOptions)
        {
            var lookup = from i in _context.JobServices
                         where i.JobMainId == 2 
                         select new
                         {
                             Value = i.Id,
                             Text = i.ActualAmt
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> JobThrusLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.JobThrus
                         orderby i.Desc
                         select new {
                             Value = i.Id,
                             Text = i.Desc
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(JobMain model, IDictionary values) {
            string ID = nameof(JobMain.Id);
            string JOB_DATE = nameof(JobMain.JobDate);
            string CUSTOMER_ID = nameof(JobMain.CustomerId);
            string DESCRIPTION = nameof(JobMain.Description);
            string NO_OF_PAX = nameof(JobMain.NoOfPax);
            string NO_OF_DAYS = nameof(JobMain.NoOfDays);
            string JOB_REMARKS = nameof(JobMain.JobRemarks);
            string JOB_STATUS_ID = nameof(JobMain.JobStatusId);
            string STATUS_REMARKS = nameof(JobMain.StatusRemarks);
            string BRANCH_ID = nameof(JobMain.BranchId);
            string JOB_THRU_ID = nameof(JobMain.JobThruId);
            string AGREED_AMT = nameof(JobMain.AgreedAmt);
            string CUST_CONTACT_EMAIL = nameof(JobMain.CustContactEmail);
            string CUST_CONTACT_NUMBER = nameof(JobMain.CustContactNumber);
            string ASSIGNED_TO = nameof(JobMain.AssignedTo);
            string DUE_DATE = nameof(JobMain.DueDate);

            if(values.Contains(ID)) {
                model.Id = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(JOB_DATE)) {
                model.JobDate = Convert.ToDateTime(values[JOB_DATE]);
            }

            if(values.Contains(CUSTOMER_ID)) {
                model.CustomerId = Convert.ToInt32(values[CUSTOMER_ID]);
            }

            if(values.Contains(DESCRIPTION)) {
                model.Description = Convert.ToString(values[DESCRIPTION]);
            }

            if(values.Contains(NO_OF_PAX)) {
                model.NoOfPax = Convert.ToInt32(values[NO_OF_PAX]);
            }

            if(values.Contains(NO_OF_DAYS)) {
                model.NoOfDays = Convert.ToInt32(values[NO_OF_DAYS]);
            }

            if(values.Contains(JOB_REMARKS)) {
                model.JobRemarks = Convert.ToString(values[JOB_REMARKS]);
            }

            if(values.Contains(JOB_STATUS_ID)) {
                model.JobStatusId = Convert.ToInt32(values[JOB_STATUS_ID]);
            }

            if(values.Contains(STATUS_REMARKS)) {
                model.StatusRemarks = Convert.ToString(values[STATUS_REMARKS]);
            }

            if(values.Contains(BRANCH_ID)) {
                model.BranchId = Convert.ToInt32(values[BRANCH_ID]);
            }

            if(values.Contains(JOB_THRU_ID)) {
                model.JobThruId = Convert.ToInt32(values[JOB_THRU_ID]);
            }

            if(values.Contains(AGREED_AMT)) {
                model.AgreedAmt = values[AGREED_AMT] != null ? Convert.ToDecimal(values[AGREED_AMT], CultureInfo.InvariantCulture) : (decimal?)null;
            }

            if(values.Contains(CUST_CONTACT_EMAIL)) {
                model.CustContactEmail = Convert.ToString(values[CUST_CONTACT_EMAIL]);
            }

            if(values.Contains(CUST_CONTACT_NUMBER)) {
                model.CustContactNumber = Convert.ToString(values[CUST_CONTACT_NUMBER]);
            }

            if(values.Contains(ASSIGNED_TO)) {
                model.AssignedTo = Convert.ToString(values[ASSIGNED_TO]);
            }

            if(values.Contains(DUE_DATE)) {
                model.DueDate = values[DUE_DATE] != null ? Convert.ToDateTime(values[DUE_DATE]) : (DateTime?)null;
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