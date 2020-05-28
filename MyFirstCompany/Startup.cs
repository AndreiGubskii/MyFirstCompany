using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyFirstCompany.Domain;
using MyFirstCompany.Domain.Entities;
using MyFirstCompany.Domain.Repositories.Abstract;
using MyFirstCompany.Domain.Repositories.EntityFramework;
using MyFirstCompany.Service;

namespace MyFirstCompany
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) => Configuration = configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //подключаем конфиг из appsettings.json
            Configuration.Bind("Project",new Config());

            //подключаем нужный функционал приложения в качестве сервисов
            services.AddTransient<ITextFieldsRepository, EFTextFieldsRepository>();
            services.AddTransient<IServiceItemsRepository, EFServiceItemsRepository>();
            services.AddTransient<DataManager>();

            //Подключаем контекст БД
            services.AddDbContext<AppDbContext>(x=>x.UseSqlServer(Config.ConnectionString));

            //настраиваем identity систему
            services.AddIdentity<IdentityUser, IdentityRole>(opts=> {
                opts.User.RequireUniqueEmail = true;
                opts.Password.RequiredLength = 6;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            //настравиваем authentification cookie
            services.ConfigureApplicationCookie(options => {
                options.Cookie.Name = "myFirstCompanyAuth";
                options.Cookie.HttpOnly = true;
                options.LoginPath = "/account/login";
                options.AccessDeniedPath = "/account/accessdenied";
                options.SlidingExpiration = true;
            });

            //добовляем поддержку конроллеров и представлений MVC
            services.AddControllersWithViews()
                //Выставляем совместимость с asp.net core 3.0
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0).AddSessionStateTempDataProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // режим отладки (подробная информация об ошибках)
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            //подключаем аутентификацию и авторизацию
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            //подключаем поддержку статческих файлов в приложении (css,js и т.д)
            app.UseStaticFiles();

            //регистрируем маршруты
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default","{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
