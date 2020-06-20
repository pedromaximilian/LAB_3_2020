using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LAB_3_2020.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LAB_3_2020.Controllers
{
    public class LoginController : Controller
    {
        private readonly My_DBContext _context;
        private readonly IConfiguration configuration;

        public LoginController(My_DBContext context, IConfiguration configuration)
        {
            _context = context;
            this.configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel u)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: u.Pass,
                    salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8
                    ));


                    var prop = await _context.Propietario
                .FirstOrDefaultAsync(m => m.Mail == u.Mail);



                    if (prop == null || prop.Password != hashed)
                    {
                        ModelState.AddModelError("", "El email o la clave no son correctas");
                        ViewBag.Error = "Verifique usuario y contraseña";
                        return View("index", u);
                    }

                    var claims = new List<Claim>
                    {


                        new Claim(ClaimTypes.Name, prop.Mail),
                        new Claim(ClaimTypes.Role, "Administrador"),
                         new Claim("Admin", "Admin")



                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims,
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction(nameof(Index), "Home");
                }
                else
                {
                    ViewBag.Error = "Usuario o contraseña invalidos";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}