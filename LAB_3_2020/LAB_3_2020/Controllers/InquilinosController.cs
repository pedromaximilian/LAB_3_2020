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
    
    public class InquilinosController : Controller
    {
        private readonly My_DBContext _context;

        public InquilinosController(My_DBContext context)
        {
            _context = context;
        }

        // GET: Inquilinos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Inquilino.ToListAsync());
        }

        // GET: Inquilinos/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inquilino = await _context.Inquilino
                .FirstOrDefaultAsync(m => m.InquilinoId == id);
            if (inquilino == null)
            {
                return NotFound();
            }

            return View(inquilino);
        }

        // GET: Inquilinos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Inquilinos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InquilinoId,Dni,Nombre,Direccion,Telefono")] Inquilino inquilino)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inquilino);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(inquilino);
        }

        // GET: Inquilinos/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inquilino = await _context.Inquilino.FindAsync(id);
            if (inquilino == null)
            {
                return NotFound();
            }
            return View(inquilino);
        }

        // POST: Inquilinos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("InquilinoId,Dni,Nombre,Direccion,Telefono")] Inquilino inquilino)
        {
            if (id != inquilino.InquilinoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inquilino);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InquilinoExists(inquilino.InquilinoId))
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
            return View(inquilino);
        }

        // GET: Inquilinos/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inquilino = await _context.Inquilino
                .FirstOrDefaultAsync(m => m.InquilinoId == id);
            if (inquilino == null)
            {
                return NotFound();
            }

            return View(inquilino);
        }

        // POST: Inquilinos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var inquilino = await _context.Inquilino.FindAsync(id);
            _context.Inquilino.Remove(inquilino);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InquilinoExists(long id)
        {
            return _context.Inquilino.Any(e => e.InquilinoId == id);
        }
    }
}
