using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace InventoryManagement
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
            var connection = Configuration.GetConnectionString("InventoryDatabase");

            IConfigurationRoot configuration = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                 .AddEnvironmentVariables()
                 .Build();

            string connectionString = Configuration.GetConnectionString("ConnectionString");

            //string connectionStringHost = configuration["EXTERNAL_AzureSqlServer_SERVICE_SERVICE_HOST"];
            //string connectionStringPort = configuration["EXTERNAL_AzureSqlServer_SERVICE_SERVICE_PORT"];
            //string connectionUser_ID = configuration["User_ID"];
            //string connectionPassword = configuration["Password"];
            //string dbConnection = string.Format("Server=tcp:{0},{3};Initial Catalog=Inventory;Persist Security Info=False;User ID={1};Password={2};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;", connectionStringHost, connectionUser_ID, connectionPassword, connectionStringPort);
            services.AddDbContext<InventoryContext>(options => options.UseSqlServer(connectionString));           

            services.AddControllersWithViews();
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

            app.UseRouting();

            app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Products}/{action=Index}/{id?}");
            //});


            app.Run(async (context) =>
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                 .AddEnvironmentVariables()
                 .Build();

                string connectionStringHost = configuration["EXTERNAL_AzureSqlServer_SERVICE_SERVICE_HOST"];
                string connectionStringPort = configuration["EXTERNAL_AzureSqlServer_SERVICE_SERVICE_PORT"];
                string connectionUser_ID = configuration["User_ID"];
                string connectionPassword = configuration["Password"];

                //var message = $"Host: {Environment.MachineName}\n" +
                //    $"EnvironmentName: {env.EnvironmentName}\n" +
                //    $"Secret value: {Configuration.GetConnectionString("InventoryDatabase")}";
                var message = $"Host: {Environment.MachineName}\n" +
                    $"EnvironmentName: {env.EnvironmentName}\n" +
                    $"connectionStringPort: {connectionStringPort}\n" +
                    $"connectionUser_ID: {connectionUser_ID}\n" +
                    $"connectionPassword: {connectionPassword}\n" +
                    $"ConnectionStringHost: {connectionStringHost}";
                await context.Response.WriteAsync(message);
            });
        }
    }
}
