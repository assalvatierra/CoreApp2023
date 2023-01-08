using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobsV1.Models;
using WebDemo.Data;
using CoreLib.Interfaces;

namespace WebDemo.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly WebDemoContext _context;
        private readonly IMainService _mainsvc;
        public SuppliersController(WebDemoContext context, IMainService mainsvc)
        {
            _context = context;
            _mainsvc = mainsvc;
        }

        // GET: Suppliers
        public async Task<IActionResult> Index()
        {
            //var webDemoContext = _context.Supplier.Include(s => s.City).Include(s => s.Country).Include(s => s.SupplierType);
            //return View(await webDemoContext.ToListAsync());

            return View(await this._mainsvc.GetSuppliers());
        }

        // GET: Suppliers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || this._mainsvc.GetSupplier((int)id) == null)
            {
                return NotFound();
            }

            //var supplier = await _context.Supplier
            //    .Include(s => s.City)
            //    .Include(s => s.Country)
            //    .Include(s => s.SupplierType)
            //    .FirstOrDefaultAsync(m => m.Id == id);
            var supplier = this._mainsvc.GetSupplier((int)id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // GET: Suppliers/Create
        public IActionResult Create()
        {
            ViewData["CityId"] = new SelectList(_context.Set<City>(), "Id", "Id");
            ViewData["CountryId"] = new SelectList(_context.Set<Country>(), "Id", "Id");
            ViewData["SupplierTypeId"] = new SelectList(_context.Set<SupplierType>(), "Id", "Id");
            return View();
        }

        // POST: Suppliers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Contact1,Contact2,Contact3,Email,Details,CityId,SupplierTypeId,Status,Website,Address,CountryId,Code")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                _context.Add(supplier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.Set<City>(), "Id", "Id", supplier.CityId);
            ViewData["CountryId"] = new SelectList(_context.Set<Country>(), "Id", "Id", supplier.CountryId);
            ViewData["SupplierTypeId"] = new SelectList(_context.Set<SupplierType>(), "Id", "Id", supplier.SupplierTypeId);
            return View(supplier);
        }

        // GET: Suppliers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Supplier == null)
            {
                return NotFound();
            }

            var supplier = await _context.Supplier.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.Set<City>(), "Id", "Id", supplier.CityId);
            ViewData["CountryId"] = new SelectList(_context.Set<Country>(), "Id", "Id", supplier.CountryId);
            ViewData["SupplierTypeId"] = new SelectList(_context.Set<SupplierType>(), "Id", "Id", supplier.SupplierTypeId);
            return View(supplier);
        }

        // POST: Suppliers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Contact1,Contact2,Contact3,Email,Details,CityId,SupplierTypeId,Status,Website,Address,CountryId,Code")] Supplier supplier)
        {
            if (id != supplier.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supplier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplierExists(supplier.Id))
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
            ViewData["CityId"] = new SelectList(_context.Set<City>(), "Id", "Id", supplier.CityId);
            ViewData["CountryId"] = new SelectList(_context.Set<Country>(), "Id", "Id", supplier.CountryId);
            ViewData["SupplierTypeId"] = new SelectList(_context.Set<SupplierType>(), "Id", "Id", supplier.SupplierTypeId);
            return View(supplier);
        }

        // GET: Suppliers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Supplier == null)
            {
                return NotFound();
            }

            var supplier = await _context.Supplier
                .Include(s => s.City)
                .Include(s => s.Country)
                .Include(s => s.SupplierType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Supplier == null)
            {
                return Problem("Entity set 'WebDemoContext.Supplier'  is null.");
            }
            var supplier = await _context.Supplier.FindAsync(id);
            if (supplier != null)
            {
                _context.Supplier.Remove(supplier);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupplierExists(int id)
        {
          return (_context.Supplier?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
