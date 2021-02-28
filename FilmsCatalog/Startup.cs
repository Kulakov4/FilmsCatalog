using FilmsCatalog.Data;
using FilmsCatalog.Interfaces;
using FilmsCatalog.Models;
using FilmsCatalog.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog
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
            string connectionString = Configuration.GetConnectionString("DefaultConnection");

            /*
            Метод AddDbContext() настраивает службы, предоставляемые инфраструктурой Entity Framework Core для класса контекста базы ApplicationDbContext. 
            Аргументом метода AddDbContext () является лямбда-выражение, которое получает объект options, конфигурирующий базу данных для класса контекста. 
            В этом случае база данных конфигурируется с помощью метода UseSqlServer() и указания строки подключения.             
             */

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddControllersWithViews();
            services.AddRazorPages();

            /*
            Transient подразумевает, что сервис создается каждый раз, когда его запрашивают. 
            Этот жизненный цикл лучше всего подходит для легковесных, не фиксирующих состояние, сервисов.             

             Scoped сервис создаются единожды для каждого запроса.

            Singleton - сервис создается при первом запросе (или при запуске ConfigureServices, если вы указываете инстанс там), 
            а затем каждый последующий запрос будет использовать этот же инстанс.
            */

            services.AddTransient<IImageProfile, PosterImageProfile>();

            services.AddScoped<FilmService>();

            // Внедряем в контроллер
            services.AddScoped<ICrudService<Film>, FilmService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
