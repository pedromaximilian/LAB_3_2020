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
    public class InmueblesController : Controller
    {
        private readonly My_DBContext _context;

        public InmueblesController(My_DBContext context)
        {
            _context = context;
        }

        // GET: Inmuebles
        public async Task<IActionResult> Index()
        {
            var my_DBContext = _context.Inmueble.Include(i => i.Propietario);
            return View(await my_DBContext.ToListAsync());
        }

        // GET: Inmuebles/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inmueble = await _context.Inmueble
                .Include(i => i.Propietario)
                .FirstOrDefaultAsync(m => m.InmuebleId == id);
            if (inmueble == null)
            {
                return NotFound();
            }

            return View(inmueble);
        }

        // GET: Inmuebles/Create
        public IActionResult Create()
        {
            ViewData["PropietarioId"] = new SelectList(_context.Propietario, "PropietarioId", "Apellido");
            return View();
        }

        // POST: Inmuebles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InmuebleId,Direccion,Ambientes,Tipo,Uso,Precio,Disponible,PropietarioId")] Inmueble inmueble)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inmueble);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PropietarioId"] = new SelectList(_context.Propietario, "PropietarioId", "Apellido", inmueble.PropietarioId);
            return View(inmueble);
        }

        // GET: Inmuebles/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inmueble = await _context.Inmueble.FindAsync(id);
            if (inmueble == null)
            {
                return NotFound();
            }
            ViewData["PropietarioId"] = new SelectList(_context.Propietario, "PropietarioId", "Apellido", inmueble.PropietarioId);
            return View(inmueble);
        }

        // POST: Inmuebles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("InmuebleId,Direccion,Ambientes,Tipo,Uso,Precio,Disponible,PropietarioId")] Inmueble inmueble)
        {
            if (id != inmueble.InmuebleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inmueble);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InmuebleExists(inmueble.InmuebleId))
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
            ViewData["PropietarioId"] = new SelectList(_context.Propietario, "PropietarioId", "Apellido", inmueble.PropietarioId);
            return View(inmueble);
        }

        // GET: Inmuebles/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inmueble = await _context.Inmueble
                .Include(i => i.Propietario)
                .FirstOrDefaultAsync(m => m.InmuebleId == id);
            if (inmueble == null)
            {
                return NotFound();
            }

            return View(inmueble);
        }

        // POST: Inmuebles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var inmueble = await _context.Inmueble.FindAsync(id);
            _context.Inmueble.Remove(inmueble);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InmuebleExists(long id)
        {
            return _context.Inmueble.Any(e => e.InmuebleId == id);
        }
    }
}
