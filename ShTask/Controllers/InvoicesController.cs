using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShTask.Models;
using ShTask.ViewModels;

namespace ShTask.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly ShaTaskContext _context;

        public InvoicesController(ShaTaskContext context)
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
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "BranchName");
            ViewData["CashierId"] = new SelectList(_context.Cashiers, "Id", "CashierName");
            return View();
        }

        // POST: InvoiceHeaders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] InvoiceHeader invoiceHeader)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        InvoiceHeader invoiceHeaderNew = new InvoiceHeader()
                        {
                            Id = invoiceHeader.Id,
                            BranchId = invoiceHeader.BranchId,
                            CashierId = invoiceHeader.CashierId,
                            Invoicedate = invoiceHeader.Invoicedate,
                            CustomerName = invoiceHeader.CustomerName,
                        };
                        _context.InvoiceHeaders.Add(invoiceHeaderNew);
                        await _context.SaveChangesAsync();

                        var lastid = _context.InvoiceHeaders.Max(x => x.Id);

                        foreach (var item in invoiceHeader.InvoiceDetails)
                        {
                            InvoiceDetail invoiceDetail = new InvoiceDetail()
                            {
                                ItemCount = item.ItemCount,
                                ItemName = item.ItemName,
                                ItemPrice = item.ItemPrice,
                                InvoiceHeaderId = lastid,
                            };
                            _context.InvoiceDetails.Add(invoiceDetail);
                        }
                        await _context.SaveChangesAsync();



                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction if an exception occurs
                        transaction.Rollback();
                        // Handle the exception
                    }
                }
                
                string url = Url.Action("Index", "Invoices", null, Request.Scheme);

                return Ok(new { url });
            }
            return BadRequest(ModelState);

        }

        // GET: InvoiceHeaders/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.InvoiceHeaders == null)
            {
                return NotFound();
            }

            var invoiceHeader = await _context.InvoiceHeaders.Include(i => i.InvoiceDetails).FirstOrDefaultAsync(a=>a.Id==id);
            if (invoiceHeader == null)
            {
                return NotFound();
            }
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "BranchName", invoiceHeader.BranchId);
            ViewData["CashierId"] = new SelectList(_context.Cashiers, "Id", "CashierName", invoiceHeader.CashierId);
            return View(invoiceHeader);
        }

        // POST: InvoiceHeaders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] InvoiceHeader invoiceHeader)
        {
            var Last = await _context.InvoiceHeaders.Include(i => i.InvoiceDetails).FirstOrDefaultAsync(a => a.Id == invoiceHeader.Id);

            if (ModelState.IsValid)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        Last.BranchId = invoiceHeader.BranchId;
                        Last.CashierId = invoiceHeader.CashierId;
                        Last.Invoicedate = invoiceHeader.Invoicedate;
                        Last.CustomerName = invoiceHeader.CustomerName;
                        Last.InvoiceDetails = new List<InvoiceDetail>();

                        var detailsToRemove = _context.InvoiceDetails.Where(d => d.InvoiceHeaderId == invoiceHeader.Id);
                        _context.InvoiceDetails.RemoveRange(detailsToRemove);
                        await _context.SaveChangesAsync();

                        foreach (var item in invoiceHeader.InvoiceDetails)
                        {
                            InvoiceDetail invoiceDetail = new InvoiceDetail()
                            {
                                ItemCount = item.ItemCount,
                                ItemName = item.ItemName,
                                ItemPrice = item.ItemPrice,
                                InvoiceHeaderId = invoiceHeader.Id,
                            };
                            _context.InvoiceDetails.Add(invoiceDetail);
                            Last.InvoiceDetails.Add(invoiceDetail);
                        }
                        _context.InvoiceHeaders.Update(Last);
                        await _context.SaveChangesAsync();



                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction if an exception occurs
                        transaction.Rollback();
                        // Handle the exception
                    }
                }

                string url = Url.Action("Index", "Invoices", null, Request.Scheme);

                return Ok(new { url });
            }
            return BadRequest(ModelState);

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

            var MainToRemove = _context.InvoiceHeaders.FirstOrDefault(d => d.Id == id);
            var detailsToRemove = _context.InvoiceDetails.Where(d => d.InvoiceHeaderId == id);

            using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        
                        _context.InvoiceHeaders.Remove(MainToRemove);
                        _context.InvoiceDetails.RemoveRange(detailsToRemove);

                        await _context.SaveChangesAsync();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction if an exception occurs
                        transaction.Rollback();
                        // Handle the exception
                    }
                }
            
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceHeaderExists(long id)
        {
            return (_context.InvoiceHeaders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
