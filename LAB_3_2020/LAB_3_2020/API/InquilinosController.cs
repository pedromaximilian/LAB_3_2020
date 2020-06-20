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
    public class InquilinosController : ControllerBase
    {
        private readonly My_DBContext _context;

        public InquilinosController(My_DBContext context)
        {
            _context = context;
        }

        // GET: api/Inquilinos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inquilino>>> GetInquilino()
        {
            return await _context.Inquilino.ToListAsync();
        }

        // GET: api/Inquilinos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Inquilino>> GetInquilino(long id)
        {
            var inquilino = await _context.Inquilino.FindAsync(id);

            if (inquilino == null)
            {
                return NotFound();
            }

            return inquilino;
        }

        // PUT: api/Inquilinos/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInquilino(long id, Inquilino inquilino)
        {
            if (id != inquilino.InquilinoId)
            {
                return BadRequest();
            }

            _context.Entry(inquilino).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InquilinoExists(id))
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

        // POST: api/Inquilinos
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Inquilino>> PostInquilino(Inquilino inquilino)
        {
            _context.Inquilino.Add(inquilino);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInquilino", new { id = inquilino.InquilinoId }, inquilino);
        }

        // DELETE: api/Inquilinos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Inquilino>> DeleteInquilino(long id)
        {
            var inquilino = await _context.Inquilino.FindAsync(id);
            if (inquilino == null)
            {
                return NotFound();
            }

            _context.Inquilino.Remove(inquilino);
            await _context.SaveChangesAsync();

            return inquilino;
        }

        private bool InquilinoExists(long id)
        {
            return _context.Inquilino.Any(e => e.InquilinoId == id);
        }
    }
}
