using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using DKAC.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Owin;
using Microsoft.Owin;
using System.Configuration;
using Hangfire;
using Hangfire.Dashboard;
using DKAC.Common;
using Microsoft.AspNet.SignalR;
using DKAC.Controllers;

[assembly: OwinStartup(typeof(DKAC.Startup))]

namespace DKAC
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();

            GlobalConfiguration.Configuration
                .UseSqlServerStorage("DKACDbContext");

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangFireAuthorizationFilter() }
            });

            RecurringJob.AddOrUpdate<HomeController>(
                "Thông báo đăng ký",
                x => x.PushNotifi(), "*/1 * * * *", TimeZoneInfo.Local);//"0 30 8 * * *" chạy lúc 8h30 hàng ngày //("*/5 * * * *")-5p chạy 1 lần

            app.UseHangfireDashboard();
            app.UseHangfireServer();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddMvcCore();
        }



        //public IConfiguration Configuration1 { get; }

        //// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        //{
        //    if (env.IsDevelopment())
        //    {
        //        app.UseMvcWithDefaultRoute(); // .UseDeveloperExceptionPage();
        //    }
        //    else
        //    {
        //        //app.UseExceptionHandler("/Error");
        //        //app.UseHsts();
        //    }

        //    //app.UseHttpsRedirection();
        //    //app.UseStaticFiles();
        //    //app.UseCookiePolicy();

        //    app.UseMvc();
        //}


    }
}
