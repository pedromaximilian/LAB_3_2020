using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LAB_3_2020.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using LAB_3_2020.Controllers;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LAB_3_2020.API
{
    //Propietarios API


    [Route("api/[controller]")]
    [ApiController]
    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PropietariosController : ControllerBase
    {
        private readonly My_DBContext _context;
        private readonly IConfiguration configuration;

        public PropietariosController(My_DBContext context, IConfiguration configuration)
        {
            _context = context;
            this.configuration = configuration;
        }

        // GET: api/Propietarios
        [HttpGet]
        public async Task<ActionResult<Propietario>> GetPropietario()
        {
            var mail = User.Identity.Name;


            Propietario p = _context.Propietario.FirstOrDefault(x => x.Mail == mail);


            return p;
        }

        // GET: api/Propietarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Propietario>> GetPropietario(long id)
        {
            var propietario = await _context.Propietario.FindAsync(id);

            if (propietario == null)
            {
                return NotFound();
            }

            return propietario;
        }

        // PUT: api/Propietarios/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut()]
        public async Task<IActionResult> PutPropietario(Propietario propietario)
        {

            if (ModelState.IsValid && propietario.Mail == User.Identity.Name)
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: propietario.Password,
                    salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8
                    ));
                propietario.Password = hashed;

                _context.Entry(propietario).State = EntityState.Modified;

                try
                {

                    await _context.SaveChangesAsync();
                    return Ok("Usuario Actualizado");
                }
                catch (Exception ex)
                {
                    throw;

                }
            }
            
            

            return BadRequest();
        }

        // POST: api/Propietarios
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Propietario>> PostPropietario(Propietario propietario)
        {
            _context.Propietario.Add(propietario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPropietario", new { id = propietario.PropietarioId }, propietario);
        }

        // DELETE: api/Propietarios/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Propietario>> DeletePropietario(long id)
        {
            var propietario = await _context.Propietario.FindAsync(id);
            if (propietario == null)
            {
                return NotFound();
            }

            _context.Propietario.Remove(propietario);
            await _context.SaveChangesAsync();

            return propietario;
        }

        private bool PropietarioExists(long id)
        {
            return _context.Propietario.Any(e => e.PropietarioId == id);
        }

    }
}
