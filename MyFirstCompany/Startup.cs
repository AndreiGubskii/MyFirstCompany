using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MyFirstCompany
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
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
