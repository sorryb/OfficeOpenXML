using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ExcelToDbfConvertor.Data;
using ExcelToDbfConvertor.Models;
using ExcelToDbfConvertor.Services;
using System;

namespace ExcelToDbfConvertor
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                //options.UseSqlServer("Server=.;Database=dbfConvertor-Users-2b29-45da-906c-f4ec4d37bbd1;Trusted_Connection=True;MultipleActiveResultSets=true")); // DefaultConnection
            options.UseSqlServer(Configuration.GetConnectionString("dbfConvertorConnection")));
            // Add database services.
            //services.AddDbContext<dbfConvertorContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("dbfConvertorConnection")));

             services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()    //
                .AddDefaultTokenProviders();

            services.AddMvc();

            // Add Database Initializer
            services.AddScoped<IDbInitializer, DbInitializer>();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IDbInitializer dbInitializer)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();

                // Browser Link is not compatible with Kestrel 1.1.0
                // For details on enabling Browser Link, see https://go.microsoft.com/fwlink/?linkid=840936
                // app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }


            app.UseStaticFiles();

            app.UseIdentity();


            ////create database
            //try     //https://stackoverflow.com/questions/38238043/how-and-where-to-call-database-ensurecreated-and-database-migrate
            //{
            //    using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
            //        .CreateScope())
            //    {
            //        dbfConvertorContext context = serviceScope.ServiceProvider.GetService<dbfConvertorContext>();
            //        context.Database.EnsureCreated();

            //        //serviceScope.ServiceProvider.GetService<dbfConvertorContext>()
            //        //    .Database.EnsureCreated();
            //        ////.Database.Migrate();
            //    }
            //}
            //catch (Exception e)
            //{
            //    var msg = e.Message;
            //    var stacktrace = e.StackTrace;
            //}

            //Generate EF Core Seed Data
            dbInitializer.Initialize();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
