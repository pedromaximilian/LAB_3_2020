using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LAB_3_2020.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LAB_3_2020.Controllers
{
    [Authorize]
    public class PagosController : Controller
    {
        private readonly My_DBContext _context;

        public PagosController(My_DBContext context)
        {
            _context = context;
        }

        // GET: Pagos
        public async Task<IActionResult> Index()
        {
            var my_DBContext = _context.Pago.Include(p => p.Alquiler);
            return View(await my_DBContext.ToListAsync());
        }

        // GET: Pagos/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pago
                .Include(p => p.Alquiler)
                .FirstOrDefaultAsync(m => m.PagoId == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        // GET: Pagos/Create
        public IActionResult Create()
        {
            ViewData["AlquilerId"] = new SelectList(_context.Alquiler, "AlquilerId", "AlquilerId");
            return View();
        }

        // POST: Pagos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PagoId,NroPago,AlquilerId,Fecha,Importe")] Pago pago)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pago);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AlquilerId"] = new SelectList(_context.Alquiler, "AlquilerId", "AlquilerId", pago.AlquilerId);
            return View(pago);
        }

        // GET: Pagos/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pago.FindAsync(id);
            if (pago == null)
            {
                return NotFound();
            }
            ViewData["AlquilerId"] = new SelectList(_context.Alquiler, "AlquilerId", "AlquilerId", pago.AlquilerId);
            return View(pago);
        }

        // POST: Pagos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("PagoId,NroPago,AlquilerId,Fecha,Importe")] Pago pago)
        {
            if (id != pago.PagoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pago);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PagoExists(pago.PagoId))
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
            ViewData["AlquilerId"] = new SelectList(_context.Alquiler, "AlquilerId", "AlquilerId", pago.AlquilerId);
            return View(pago);
        }

        // GET: Pagos/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pago
                .Include(p => p.Alquiler)
                .FirstOrDefaultAsync(m => m.PagoId == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        // POST: Pagos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var pago = await _context.Pago.FindAsync(id);
            _context.Pago.Remove(pago);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PagoExists(long id)
        {
            return _context.Pago.Any(e => e.PagoId == id);
        }
    }
}
