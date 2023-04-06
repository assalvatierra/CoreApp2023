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
    public class CustEntAddressesController : Controller
    {
        private readonly eJobContext _context;

        public CustEntAddressesController(eJobContext context)
        {
            _context = context;
        }

        // GET: Companies/CustEntAddresses
        public async Task<IActionResult> Index()
        {
            var eJobContext = _context.CustEntAddress.Include(c => c.CustEntMain);
            return View(await eJobContext.ToListAsync());
        }

        // GET: Companies/CustEntAddresses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CustEntAddress == null)
            {
                return NotFound();
            }

            var custEntAddress = await _context.CustEntAddress
                .Include(c => c.CustEntMain)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (custEntAddress == null)
            {
                return NotFound();
            }

            return View(custEntAddress);
        }

        // GET: Companies/CustEntAddresses/Create
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

        // POST: Companies/CustEntAddresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustEntMainId,Line1,Line2,Line3,Line4,Line5,isBilling,isPrimary")] CustEntAddress custEntAddress)
        {
            if (ModelState.IsValid)
            {
                _context.Add(custEntAddress);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Details", "CustEntMains", new { id = custEntAddress.CustEntMainId });
            }
            ViewData["CustEntMainId"] = new SelectList(_context.Set<CustEntMain>(), "Id", "Id", custEntAddress.CustEntMainId);
            ViewBag.CompanyId = custEntAddress.CustEntMainId;
            return View(custEntAddress);
        }

        // GET: Companies/CustEntAddresses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CustEntAddress == null)
            {
                return NotFound();
            }

            var custEntAddress = await _context.CustEntAddress.FindAsync(id);
            if (custEntAddress == null)
            {
                return NotFound();
            }
            ViewData["CustEntMainId"] = new SelectList(_context.Set<CustEntMain>(), "Id", "Id", custEntAddress.CustEntMainId);
            ViewBag.CompanyId = custEntAddress.CustEntMainId;
            return View(custEntAddress);
        }

        // POST: Companies/CustEntAddresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustEntMainId,Line1,Line2,Line3,Line4,Line5,isBilling,isPrimary")] CustEntAddress custEntAddress)
        {
            if (id != custEntAddress.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(custEntAddress);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustEntAddressExists(custEntAddress.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction(nameof(Index));

                return RedirectToAction("Details", "CustEntMains", new { id = custEntAddress.CustEntMainId });
            }
            ViewData["CustEntMainId"] = new SelectList(_context.Set<CustEntMain>(), "Id", "Id", custEntAddress.CustEntMainId);
            ViewBag.CompanyId = custEntAddress.CustEntMainId;
            return View(custEntAddress);
        }

        // GET: Companies/CustEntAddresses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CustEntAddress == null)
            {
                return NotFound();
            }

            var custEntAddress = await _context.CustEntAddress
                .Include(c => c.CustEntMain)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (custEntAddress == null)
            {
                return NotFound();
            }

            return View(custEntAddress);
        }

        // POST: Companies/CustEntAddresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CustEntAddress == null)
            {
                return Problem("Entity set 'eJobContext.CustEntAddress'  is null.");
            }
            var custEntAddress = await _context.CustEntAddress.FindAsync(id);
            if (custEntAddress != null)
            {
                _context.CustEntAddress.Remove(custEntAddress);
            }
            
            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Details", "CustEntMains", new { id = custEntAddress.CustEntMainId });
        }

        private bool CustEntAddressExists(int id)
        {
          return (_context.CustEntAddress?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
