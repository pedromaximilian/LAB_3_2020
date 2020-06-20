using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LAB_3_2020.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace LAB_3_2020.Controllers
{

    //propietario web
    [Authorize]
    public class PropietariosController : Controller
    {
        private readonly My_DBContext _context;
        private readonly IConfiguration configuration;

        public PropietariosController(My_DBContext context, IConfiguration configuration)
        {
            _context = context;
            this.configuration = configuration;
        }

        // GET: Propietarios WEB
        public async Task<IActionResult> Index()
        {
            return View(await _context.Propietario.ToListAsync());
        }

        // GET: Propietarios/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propietario = await _context.Propietario
                .FirstOrDefaultAsync(m => m.PropietarioId == id);
            if (propietario == null)
            {
                return NotFound();
            }

            return View(propietario);
        }

        // GET: Propietarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Propietarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PropietarioId,Dni,Apellido,Nombre,Telefono,Mail,Password")] Propietario propietario)
        {
            if (ModelState.IsValid)
            {
                var prop = await _context.Propietario
                .FirstOrDefaultAsync(m => m.Mail == propietario.Mail);
                if (prop != null)
                {
                    ViewBag.Error = "El Propietario ya existe.";
                    return View();
                }

                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: propietario.Password,
                    salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8
                    ));
                propietario.Password = hashed;

                _context.Add(propietario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(propietario);
        }

        // GET: Propietarios/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propietario = await _context.Propietario.FindAsync(id);
            if (propietario == null)
            {
                return NotFound();
            }
            return View(propietario);
        }

        // POST: Propietarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("PropietarioId,Dni,Apellido,Nombre,Telefono,Mail,Password")] Propietario propietario)
        {
            if (id != propietario.PropietarioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(propietario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PropietarioExists(propietario.PropietarioId))
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
            return View(propietario);
        }

        // GET: Propietarios/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propietario = await _context.Propietario
                .FirstOrDefaultAsync(m => m.PropietarioId == id);
            if (propietario == null)
            {
                return NotFound();
            }

            return View(propietario);
        }

        // POST: Propietarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var propietario = await _context.Propietario.FindAsync(id);
            _context.Propietario.Remove(propietario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PropietarioExists(long id)
        {
            return _context.Propietario.Any(e => e.PropietarioId == id);
        }
    }
}
