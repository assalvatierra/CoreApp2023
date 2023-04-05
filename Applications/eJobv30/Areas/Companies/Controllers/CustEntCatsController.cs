using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RealSys.CoreLib.Models.Erp;
using eJobv30.Data;

namespace eJobv30.Areas.Companies.Controllers
{
    [Area("Companies")]
    public class CustEntCatsController : Controller
    {
        private readonly eJobContext _context;

        public CustEntCatsController(eJobContext context)
        {
            _context = context;
        }

        // GET: Companies/CustEntCats
        public async Task<IActionResult> Index()
        {
            var eJobContext = _context.CustEntCat.Include(c => c.CustCategory).Include(c => c.CustEntMain);
            return View(await eJobContext.ToListAsync());
        }

        // GET: Companies/CustEntCats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CustEntCat == null)
            {
                return NotFound();
            }

            var custEntCat = await _context.CustEntCat
                .Include(c => c.CustCategory)
                .Include(c => c.CustEntMain)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (custEntCat == null)
            {
                return NotFound();
            }

            return View(custEntCat);
        }

        // GET: Companies/CustEntCats/Create
        public IActionResult Create()
        {
            ViewData["CustCategoryId"] = new SelectList(_context.CustCategory, "Id", "Id");
            ViewData["CustEntMainId"] = new SelectList(_context.Set<CustEntMain>(), "Id", "Id");
            return View();
        }

        // POST: Companies/CustEntCats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustEntMainId,CustCategoryId")] CustEntCat custEntCat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(custEntCat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustCategoryId"] = new SelectList(_context.CustCategory, "Id", "Id", custEntCat.CustCategoryId);
            ViewData["CustEntMainId"] = new SelectList(_context.Set<CustEntMain>(), "Id", "Id", custEntCat.CustEntMainId);
            return View(custEntCat);
        }

        // GET: Companies/CustEntCats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CustEntCat == null)
            {
                return NotFound();
            }

            var custEntCat = await _context.CustEntCat.FindAsync(id);
            if (custEntCat == null)
            {
                return NotFound();
            }
            ViewData["CustCategoryId"] = new SelectList(_context.CustCategory, "Id", "Id", custEntCat.CustCategoryId);
            ViewData["CustEntMainId"] = new SelectList(_context.Set<CustEntMain>(), "Id", "Id", custEntCat.CustEntMainId);
            return View(custEntCat);
        }

        // POST: Companies/CustEntCats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustEntMainId,CustCategoryId")] CustEntCat custEntCat)
        {
            if (id != custEntCat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(custEntCat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustEntCatExists(custEntCat.Id))
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
            ViewData["CustCategoryId"] = new SelectList(_context.CustCategory, "Id", "Id", custEntCat.CustCategoryId);
            ViewData["CustEntMainId"] = new SelectList(_context.Set<CustEntMain>(), "Id", "Id", custEntCat.CustEntMainId);
            return View(custEntCat);
        }

        // GET: Companies/CustEntCats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CustEntCat == null)
            {
                return NotFound();
            }

            var custEntCat = await _context.CustEntCat
                .Include(c => c.CustCategory)
                .Include(c => c.CustEntMain)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (custEntCat == null)
            {
                return NotFound();
            }

            return View(custEntCat);
        }

        // POST: Companies/CustEntCats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CustEntCat == null)
            {
                return Problem("Entity set 'eJobContext.CustEntCat'  is null.");
            }
            var custEntCat = await _context.CustEntCat.FindAsync(id);
            if (custEntCat != null)
            {
                _context.CustEntCat.Remove(custEntCat);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustEntCatExists(int id)
        {
          return (_context.CustEntCat?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
