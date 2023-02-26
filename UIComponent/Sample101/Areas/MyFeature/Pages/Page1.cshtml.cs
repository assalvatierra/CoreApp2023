using CoreLib.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Sample101.MyFeature.Pages
{
    public class Page1Model : PageModel
    {
        private readonly CoreDBContext dbcontext;
        public Page1Model(CoreDBContext _context)
        {
            this.dbcontext = _context;
        }
        public void OnGet()
        {
            var data = this.dbcontext.Suppliers.ToList();
            var i = 0;

        }
    }
}