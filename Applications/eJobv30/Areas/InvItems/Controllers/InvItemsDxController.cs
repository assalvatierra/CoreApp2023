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
    public class InvItemsDxController : Controller
    {
        private ErpDbContext _context;

        public InvItemsDxController(ErpDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            var invitems = _context.InvItems.Select(i => new
            {
                i.Id,
                i.ItemCode,
                i.Description,
                i.Remarks,
                i.ImgPath,
                i.ContactInfo,
                i.ViewLabel,
                i.OrderNo
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "Id" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(invitems, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values)
        {
            var model = new InvItem();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if (!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.InvItems.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.Id });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values)
        {
            var model = await _context.InvItems.FirstOrDefaultAsync(item => item.Id == key);
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
            var model = await _context.InvItems.FirstOrDefaultAsync(item => item.Id == key);

            _context.InvItems.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(InvItem model, IDictionary values)
        {
            string ID = nameof(InvItem.Id);
            string ITEM_CODE = nameof(InvItem.ItemCode);
            string DESCRIPTION = nameof(InvItem.Description);
            string REMARKS = nameof(InvItem.Remarks);
            string IMG_PATH = nameof(InvItem.ImgPath);
            string CONTACT_INFO = nameof(InvItem.ContactInfo);
            string VIEW_LABEL = nameof(InvItem.ViewLabel);
            string ORDER_NO = nameof(InvItem.OrderNo);

            if (values.Contains(ID))
            {
                model.Id = Convert.ToInt32(values[ID]);
            }

            if (values.Contains(ITEM_CODE))
            {
                model.ItemCode = Convert.ToString(values[ITEM_CODE]);
            }

            if (values.Contains(DESCRIPTION))
            {
                model.Description = Convert.ToString(values[DESCRIPTION]);
            }

            if (values.Contains(REMARKS))
            {
                model.Remarks = Convert.ToString(values[REMARKS]);
            }

            if (values.Contains(IMG_PATH))
            {
                model.ImgPath = Convert.ToString(values[IMG_PATH]);
            }

            if (values.Contains(CONTACT_INFO))
            {
                model.ContactInfo = Convert.ToString(values[CONTACT_INFO]);
            }

            if (values.Contains(VIEW_LABEL))
            {
                model.ViewLabel = Convert.ToString(values[VIEW_LABEL]);
            }

            if (values.Contains(ORDER_NO))
            {
                model.OrderNo = values[ORDER_NO] != null ? Convert.ToInt32(values[ORDER_NO]) : null;
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