using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LAB_3_2020.Models;
using Microsoft.AspNetCore.Authorization;

namespace LAB_3_2020.Controllers
{
    [Authorize]
    public class AlquileresController : Controller
    {
        private readonly My_DBContext _context;

        public AlquileresController(My_DBContext context)
        {
            _context = context;
        }

        // GET: Alquileres
        public async Task<IActionResult> Index()
        {
            var my_DBContext = _context.Alquiler.Include(a => a.Inmueble).Include(a => a.Inquilino);
            return View(await my_DBContext.ToListAsync());
        }

        // GET: Alquileres/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alquiler = await _context.Alquiler
                .Include(a => a.Inmueble)
                .Include(a => a.Inquilino)
                .FirstOrDefaultAsync(m => m.AlquilerId == id);
            if (alquiler == null)
            {
                return NotFound();
            }

            return View(alquiler);
        }

        // GET: Alquileres/Create
        public IActionResult Create()
        {
            ViewData["InmuebleId"] = new SelectList(_context.Inmueble, "InmuebleId", "Direccion");
            ViewData["InquilinoId"] = new SelectList(_context.Inquilino, "InquilinoId", "InquilinoId");
            return View();
        }

        // POST: Alquileres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AlquilerId,Precio,Inicio,Fin,InquilinoId,InmuebleId")] Alquiler alquiler)
        {
            if (ModelState.IsValid)
            {
                _context.Add(alquiler);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InmuebleId"] = new SelectList(_context.Inmueble, "InmuebleId", "Direccion", alquiler.InmuebleId);
            ViewData["InquilinoId"] = new SelectList(_context.Inquilino, "InquilinoId", "InquilinoId", alquiler.InquilinoId);
            return View(alquiler);
        }

        // GET: Alquileres/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alquiler = await _context.Alquiler.FindAsync(id);
            if (alquiler == null)
            {
                return NotFound();
            }
            ViewData["InmuebleId"] = new SelectList(_context.Inmueble, "InmuebleId", "Direccion", alquiler.InmuebleId);
            ViewData["InquilinoId"] = new SelectList(_context.Inquilino, "InquilinoId", "InquilinoId", alquiler.InquilinoId);
            return View(alquiler);
        }

        // POST: Alquileres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("AlquilerId,Precio,Inicio,Fin,InquilinoId,InmuebleId")] Alquiler alquiler)
        {
            if (id != alquiler.AlquilerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(alquiler);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlquilerExists(alquiler.AlquilerId))
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
            ViewData["InmuebleId"] = new SelectList(_context.Inmueble, "InmuebleId", "Direccion", alquiler.InmuebleId);
            ViewData["InquilinoId"] = new SelectList(_context.Inquilino, "InquilinoId", "InquilinoId", alquiler.InquilinoId);
            return View(alquiler);
        }

        // GET: Alquileres/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alquiler = await _context.Alquiler
                .Include(a => a.Inmueble)
                .Include(a => a.Inquilino)
                .FirstOrDefaultAsync(m => m.AlquilerId == id);
            if (alquiler == null)
            {
                return NotFound();
            }

            return View(alquiler);
        }

        // POST: Alquileres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var alquiler = await _context.Alquiler.FindAsync(id);
            _context.Alquiler.Remove(alquiler);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlquilerExists(long id)
        {
            return _context.Alquiler.Any(e => e.AlquilerId == id);
        }
    }
}
