using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobsV1.Models;
using SampleWeb.Data;

namespace SampleWeb.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly SampleWebContext _context;
        private Core.Supplier.Supplier sup;
        private API.SuppliersController api_sup;
        public SuppliersController(SampleWebContext context)
        {
            _context = context;
            this.sup = new Core.Supplier.Supplier(context);
            this.api_sup = new API.SuppliersController(context);
        }

        // GET: Suppliers
        public async Task<IActionResult> Index()
        {
            //var sampleWebContext = _context.Suppliers.Include(s => s.City).Include(s => s.Country).Include(s => s.SupplierType);
            //return View(await sampleWebContext.ToListAsync());
            //var sampleWebContext = this.sup.GetSuppliers();

            // using Library to retrive Suppliers
            //return View(this.sup.GetSuppliers()
            //    .Include(s => s.City)
            //    .Include(s => s.Country)
            //    .Include(s => s.SupplierType)
            //    );

            // Using API to retrieve suppliers
            return View(this.api_sup.GetSuppliers());
        }

        // GET: Suppliers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Suppliers == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers
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
            if (id == null || _context.Suppliers == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers.FindAsync(id);
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
            if (id == null || _context.Suppliers == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers
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
            if (_context.Suppliers == null)
            {
                return Problem("Entity set 'SampleWebContext.Supplier'  is null.");
            }
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier != null)
            {
                _context.Suppliers.Remove(supplier);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupplierExists(int id)
        {
          return (_context.Suppliers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
