using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RealSys.CoreLib.Models.Erp;
using eJobv30.Data;
using System.ComponentModel.Design;

namespace eJobv30.Areas.Companies.Controllers
{
    [Area("Companies")]
    public class CustEntClausesController : Controller
    {
        private readonly eJobContext _context;

        public CustEntClausesController(eJobContext context)
        {
            _context = context;
        }

        // GET: Companies/CustEntClauses
        public async Task<IActionResult> Index()
        {
            var eJobContext = _context.CustEntClauses.Include(c => c.CustEntMain);
            return View(await eJobContext.ToListAsync());
        }

        // GET: Companies/CustEntClauses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CustEntClauses == null)
            {
                return NotFound();
            }

            var custEntClauses = await _context.CustEntClauses
                .Include(c => c.CustEntMain)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (custEntClauses == null)
            {
                return NotFound();
            }

            return View(custEntClauses);
        }

        // GET: Companies/CustEntClauses/Create
        public IActionResult Create(int? companyId)
        {
            if (companyId == null)
            {
                return NotFound();
            }

            ViewData["CustEntMainId"] = new SelectList(_context.Set<CustEntMain>(), "Id", "Id", companyId);

            ViewBag.CompanyId = companyId;
            return View();
        }

        // POST: Companies/CustEntClauses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustEntMainId,Title,ValidStart,ValidEnd,Desc1,Desc2,Desc3,DtEncoded,EncodedBy")] CustEntClauses custEntClauses)
        {
            if (ModelState.IsValid)
            {
                _context.Add(custEntClauses);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Details", "CustEntMains", new { id = custEntClauses.CustEntMainId });
            }
            ViewData["CustEntMainId"] = new SelectList(_context.Set<CustEntMain>(), "Id", "Id", custEntClauses.CustEntMainId);
            ViewBag.CompanyId = custEntClauses.CustEntMainId;
            return View(custEntClauses);
        }

        // GET: Companies/CustEntClauses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CustEntClauses == null)
            {
                return NotFound();
            }

            var custEntClauses = await _context.CustEntClauses.FindAsync(id);
            if (custEntClauses == null)
            {
                return NotFound();
            }
            ViewData["CustEntMainId"] = new SelectList(_context.Set<CustEntMain>(), "Id", "Id", custEntClauses.CustEntMainId);
            ViewBag.CompanyId = custEntClauses.CustEntMainId;
            return View(custEntClauses);
        }

        // POST: Companies/CustEntClauses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustEntMainId,Title,ValidStart,ValidEnd,Desc1,Desc2,Desc3,DtEncoded,EncodedBy")] CustEntClauses custEntClauses)
        {
            if (id != custEntClauses.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(custEntClauses);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustEntClausesExists(custEntClauses.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "CustEntMains", new { id = custEntClauses.CustEntMainId });
            }
            ViewData["CustEntMainId"] = new SelectList(_context.Set<CustEntMain>(), "Id", "Id", custEntClauses.CustEntMainId);
            ViewBag.CompanyId = custEntClauses.CustEntMainId;
            return View(custEntClauses);
        }

        // GET: Companies/CustEntClauses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CustEntClauses == null)
            {
                return NotFound();
            }

            var custEntClauses = await _context.CustEntClauses
                .Include(c => c.CustEntMain)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (custEntClauses == null)
            {
                return NotFound();
            }

            return View(custEntClauses);
        }

        // POST: Companies/CustEntClauses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CustEntClauses == null)
            {
                return Problem("Entity set 'eJobContext.CustEntClauses'  is null.");
            }
            var custEntClauses = await _context.CustEntClauses.FindAsync(id);
            if (custEntClauses != null)
            {
                _context.CustEntClauses.Remove(custEntClauses);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "CustEntMains", new { id = custEntClauses.CustEntMainId });
        }

        private bool CustEntClausesExists(int id)
        {
          return (_context.CustEntClauses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
