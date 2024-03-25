using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShTask.Models;

namespace ShTask.Controllers
{
    public class InvoiceHeadersController : Controller
    {
        private readonly ShaTaskContext _context;

        public InvoiceHeadersController(ShaTaskContext context)
        {
            _context = context;
        }

        // GET: InvoiceHeaders
        public async Task<IActionResult> Index()
        {
            var shaTaskContext = _context.InvoiceHeaders.Include(i => i.Branch).Include(i => i.Cashier).Include(i => i.InvoiceDetails).Include(i => i.Branch.City);
            return View(await shaTaskContext.ToListAsync());
        }

        // GET: InvoiceHeaders/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.InvoiceHeaders == null)
            {
                return NotFound();
            }

            var invoiceHeader = await _context.InvoiceHeaders
                .Include(i => i.Branch)
                .Include(i => i.Cashier)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoiceHeader == null)
            {
                return NotFound();
            }

            return View(invoiceHeader);
        }

        // GET: InvoiceHeaders/Create
        public IActionResult Create()
        {
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Id");
            ViewData["CashierId"] = new SelectList(_context.Cashiers, "Id", "Id");
            return View();
        }

        // POST: InvoiceHeaders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustomerName,Invoicedate,CashierId,BranchId")] InvoiceHeader invoiceHeader)
        {
            if (ModelState.IsValid)
            {
                _context.Add(invoiceHeader);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Id", invoiceHeader.BranchId);
            ViewData["CashierId"] = new SelectList(_context.Cashiers, "Id", "Id", invoiceHeader.CashierId);
            return View(invoiceHeader);
        }

        // GET: InvoiceHeaders/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.InvoiceHeaders == null)
            {
                return NotFound();
            }

            var invoiceHeader = await _context.InvoiceHeaders.FindAsync(id);
            if (invoiceHeader == null)
            {
                return NotFound();
            }
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Id", invoiceHeader.BranchId);
            ViewData["CashierId"] = new SelectList(_context.Cashiers, "Id", "Id", invoiceHeader.CashierId);
            return View(invoiceHeader);
        }

        // POST: InvoiceHeaders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,CustomerName,Invoicedate,CashierId,BranchId")] InvoiceHeader invoiceHeader)
        {
            if (id != invoiceHeader.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoiceHeader);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceHeaderExists(invoiceHeader.Id))
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
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Id", invoiceHeader.BranchId);
            ViewData["CashierId"] = new SelectList(_context.Cashiers, "Id", "Id", invoiceHeader.CashierId);
            return View(invoiceHeader);
        }

        // GET: InvoiceHeaders/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.InvoiceHeaders == null)
            {
                return NotFound();
            }

            var invoiceHeader = await _context.InvoiceHeaders
                .Include(i => i.Branch)
                .Include(i => i.Cashier)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoiceHeader == null)
            {
                return NotFound();
            }

            return View(invoiceHeader);
        }

        // POST: InvoiceHeaders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.InvoiceHeaders == null)
            {
                return Problem("Entity set 'ShaTaskContext.InvoiceHeaders'  is null.");
            }
            var invoiceHeader = await _context.InvoiceHeaders.FindAsync(id);
            if (invoiceHeader != null)
            {
                _context.InvoiceHeaders.Remove(invoiceHeader);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceHeaderExists(long id)
        {
          return (_context.InvoiceHeaders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
