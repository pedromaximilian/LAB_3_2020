using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LAB_3_2020.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace LAB_3_2020.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AlquileresController : ControllerBase
    {
        private readonly My_DBContext _context;

        public AlquileresController(My_DBContext context)
        {
            _context = context;
        }

        // GET: api/Alquileres
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Alquiler>>> GetAlquiler()
        {
            
            return await _context.Alquiler.Include(a => a.Inmueble).Include(a => a.Inquilino).ToListAsync();
        }

        // GET: api/Alquileres/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Alquiler>> GetAlquiler(long id)
        {
            var alquiler = await _context.Alquiler
                .Include(a => a.Inmueble)
                .Include(a => a.Inquilino)
                .Include(a => a.Pago)
                .FirstOrDefaultAsync(m => m.AlquilerId == id);

            if (alquiler == null)
            {
                return NotFound();
            }

            return alquiler;
        }

        // PUT: api/Alquileres/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAlquiler(long id, Alquiler alquiler)
        {
            if (id != alquiler.AlquilerId)
            {
                return BadRequest();
            }

            _context.Entry(alquiler).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlquilerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Alquileres
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Alquiler>> PostAlquiler(Alquiler alquiler)
        {
            _context.Alquiler.Add(alquiler);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAlquiler", new { id = alquiler.AlquilerId }, alquiler);
        }

        // DELETE: api/Alquileres/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Alquiler>> DeleteAlquiler(long id)
        {
            var alquiler = await _context.Alquiler.FindAsync(id);
            if (alquiler == null)
            {
                return NotFound();
            }

            _context.Alquiler.Remove(alquiler);
            await _context.SaveChangesAsync();

            return alquiler;
        }

        private bool AlquilerExists(long id)
        {
            return _context.Alquiler.Any(e => e.AlquilerId == id);
        }
    }
}
