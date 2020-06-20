using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace LAB_3_2020
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>//el sitio web valida con cookie
                {
                    options.LoginPath = "/Login/Index";
                    options.LogoutPath = "/Login/Logout";
                    options.AccessDeniedPath = "/Login/Index";
                })

                .AddJwtBearer(options =>//la api web valida con token
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["TokenAuthentication:Issuer"],
                        ValidAudience = Configuration["TokenAuthentication:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(Configuration["TokenAuthentication:SecretKey"])),
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrador", policy =>
                //policy.RequireClaim(ClaimTypes.Role, "Administrador")
                policy.RequireClaim("Admin")
                );

            });
            services.AddControllersWithViews();



            //comando scaffolding
            //Scaffold-DbContext "Server=HP-PRINTER-9856;Initial Catalog=API_Lab3;Integrated Security=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models



            // Hay que registrar manualmente el contexto, el scaffolding no lo hace por defecto
            services.AddControllers().AddNewtonsoftJson(options =>// para core 3.1
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore); // para core 3.1
            services.AddConnections();
            services.AddDbContext<Models.My_DBContext>(options =>
              options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));
            services.AddMvc()
                     .AddControllersAsServices();
            services.AddControllers().AddControllersAsServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
