using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RealSys.CoreLib.Models.Erp;
using eJobv30.Data;

namespace eJobv30.Areas.Customers.Controllers
{
    [Area("Customers")]
    public class CustCategoriesController : Controller
    {
        private readonly ErpDbContext _context;

        public CustCategoriesController(ErpDbContext context)
        {
            _context = context;
        }

        // GET: Customers/CustCategories
        public async Task<IActionResult> Index()
        {
              return _context.CustCategories != null ? 
                          View(await _context.CustCategories.ToListAsync()) :
                          Problem("Entity set 'eJobContext.CustCategory'  is null.");
        }

        // GET: Customers/CustCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CustCategories == null)
            {
                return NotFound();
            }

            var custCategory = await _context.CustCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (custCategory == null)
            {
                return NotFound();
            }

            return View(custCategory);
        }

        // GET: Customers/CustCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/CustCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,iconPath")] CustCategory custCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(custCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(custCategory);
        }

        // GET: Customers/CustCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CustCategories == null)
            {
                return NotFound();
            }

            var custCategory = await _context.CustCategories.FindAsync(id);
            if (custCategory == null)
            {
                return NotFound();
            }
            return View(custCategory);
        }

        // POST: Customers/CustCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,iconPath")] CustCategory custCategory)
        {
            if (id != custCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(custCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustCategoryExists(custCategory.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(custCategory);
        }

        // GET: Customers/CustCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CustCategories == null)
            {
                return NotFound();
            }

            var custCategory = await _context.CustCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (custCategory == null)
            {
                return NotFound();
            }

            return View(custCategory);
        }

        // POST: Customers/CustCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CustCategories == null)
            {
                return Problem("Entity set 'eJobContext.CustCategory'  is null.");
            }
            var custCategory = await _context.CustCategories.FindAsync(id);
            if (custCategory != null)
            {
                _context.CustCategories.Remove(custCategory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustCategoryExists(int id)
        {
          return (_context.CustCategories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
