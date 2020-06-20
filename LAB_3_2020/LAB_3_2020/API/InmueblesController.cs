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
using System.Security.Cryptography.X509Certificates;

namespace LAB_3_2020.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class InmueblesController : ControllerBase
    {
        private readonly My_DBContext _context;

        public InmueblesController(My_DBContext context)
        {
            _context = context;
        }

        // GET: api/Inmuebles
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Inmueble>>> GetInmueble()
        //{
        //    var mail = User.Identity.Name;

        //    Propietario p = _context.Propietario.FirstOrDefault(x => x.Mail == mail);

        //    return await _context.Inmueble.Where(x => x.PropietarioId == p.PropietarioId);
        //}


        [HttpGet]
        public async Task<IActionResult> GetInmueble()
        {
            try
            {
                var mail = User.Identity.Name;
                return Ok(_context.Inmueble.Where(e => e.Propietario.Mail == mail));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }



        // GET: api/Inmuebles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Inmueble>> GetInmueble(long id)
        {
            var inmueble = await _context.Inmueble.FindAsync(id);

            if (inmueble == null)
            {
                return NotFound();
            }

            return inmueble;
        }

        // PUT: api/Inmuebles/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut()]
        public async Task<IActionResult> PutInmueble(Inmueble inmueble)
        {
            try
            {

                Propietario p = _context.Propietario.AsNoTracking().FirstOrDefault(x => x.Mail == User.Identity.Name);
                inmueble.PropietarioId = p.PropietarioId;
                
                if (ModelState.IsValid && _context.Inmueble.AsNoTracking().SingleOrDefault(x => x.InmuebleId == inmueble.InmuebleId) != null)
                {

                    _context.Inmueble.Update(inmueble);
                    _context.SaveChanges();
                    return Ok(inmueble);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // POST: api/Inmuebles
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<IActionResult> PostInmueble(Inmueble entidad)
        {

            try
            {
                

                if (ModelState.IsValid)
                {

                    entidad.PropietarioId = _context.Propietario.Single(e => e.Mail == User.Identity.Name).PropietarioId;
                    _context.Inmueble.Add(entidad);
                    _context.SaveChanges();
                    return CreatedAtAction(nameof(GetInmueble), new { id = entidad.InmuebleId }, entidad);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // DELETE: api/Inmuebles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Inmueble>> DeleteInmueble(long id)
        {
            try
            {
                Propietario p = _context.Propietario.SingleOrDefault(x => x.Mail == User.Identity.Name);
                var inmueble = await _context.Inmueble.FindAsync(id);
                if (inmueble.PropietarioId != p.PropietarioId)
                {
                    return BadRequest();
                }
                if (inmueble == null )
                {
                    return NotFound();
                }

                _context.Inmueble.Remove(inmueble);
                await _context.SaveChangesAsync();

                return inmueble;
            }
            catch (Exception e)
            {

                return BadRequest(e);
            }


        }

        private bool InmuebleExists(long id)
        {
            return _context.Inmueble.Any(e => e.InmuebleId == id);
        }
    }
}
