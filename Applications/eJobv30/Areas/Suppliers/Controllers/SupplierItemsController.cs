using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RealSys.CoreLib.Models.Erp;
using eJobv30.Data;

namespace eJobv30.Areas.Suppliers.Controllers
{
    [Area("Suppliers")]
    public class SupplierItemsController : Controller
    {
        private readonly ErpDbContext _context;

        public SupplierItemsController(ErpDbContext context)
        {
            _context = context;
        }

        // GET: Suppliers/SupplierItems
        public async Task<IActionResult> Index()
        {
            var eJobContext = _context.SupplierItems.Include(s => s.Supplier);
            return View(await eJobContext.ToListAsync());
        }

        // GET: Suppliers/SupplierItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SupplierItems == null)
            {
                return NotFound();
            }

            var supplierItem = await _context.SupplierItems
                .Include(s => s.Supplier)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supplierItem == null)
            {
                return NotFound();
            }

            return View(supplierItem);
        }

        // GET: Suppliers/SupplierItems/Create
        public IActionResult Create()
        {
            ViewData["SupplierId"] = new SelectList(_context.Set<Supplier>(), "Id", "Id");
            return View();
        }

        // POST: Suppliers/SupplierItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,Remarks,SupplierId,InCharge,Tel1,Tel2,Tel3,Status,Interval")] SupplierItem supplierItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(supplierItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SupplierId"] = new SelectList(_context.Set<Supplier>(), "Id", "Id", supplierItem.SupplierId);
            return View(supplierItem);
        }

        // GET: Suppliers/SupplierItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SupplierItems == null)
            {
                return NotFound();
            }

            var supplierItem = await _context.SupplierItems.FindAsync(id);
            if (supplierItem == null)
            {
                return NotFound();
            }
            ViewData["SupplierId"] = new SelectList(_context.Set<Supplier>(), "Id", "Id", supplierItem.SupplierId);
            return View(supplierItem);
        }

        // POST: Suppliers/SupplierItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Remarks,SupplierId,InCharge,Tel1,Tel2,Tel3,Status,Interval")] SupplierItem supplierItem)
        {
            if (id != supplierItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supplierItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplierItemExists(supplierItem.Id))
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
            ViewData["SupplierId"] = new SelectList(_context.Set<Supplier>(), "Id", "Id", supplierItem.SupplierId);
            return View(supplierItem);
        }

        // GET: Suppliers/SupplierItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SupplierItems == null)
            {
                return NotFound();
            }

            var supplierItem = await _context.SupplierItems
                .Include(s => s.Supplier)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supplierItem == null)
            {
                return NotFound();
            }

            return View(supplierItem);
        }

        // POST: Suppliers/SupplierItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SupplierItems == null)
            {
                return Problem("Entity set 'eJobContext.SupplierItem'  is null.");
            }
            var supplierItem = await _context.SupplierItems.FindAsync(id);
            if (supplierItem != null)
            {
                _context.SupplierItems.Remove(supplierItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupplierItemExists(int id)
        {
          return (_context.SupplierItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
