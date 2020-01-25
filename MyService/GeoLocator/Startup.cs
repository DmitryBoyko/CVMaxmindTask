using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace GeoLocator
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

			var connectionString = Configuration["PostgreSql:ConnectionString"];
			var dbPassword = Configuration["PostgreSql:DbPassword"];

			var builder = new NpgsqlConnectionStringBuilder(connectionString)
			{
				Password = dbPassword
			};

			//https://stackoverflow.com/questions/57684093/using-usemvc-to-configure-mvc-is-not-supported-while-using-endpoint-routing
			services.AddMvc(option => option.EnableEndpointRouting = false);

			services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(builder.ConnectionString));
		}

		// [System.Obsolete]
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			/* if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			} */

			app.UseHttpsRedirection();
			app.UseMvc();

			//app.UseRouting();

			/* app.UseEndpoints(endpoints =>
			{
				endpoints.MapGet("/", async context =>
				{
					await context.Response.WriteAsync("Hello World!");
				});
			});*/

			/*
			 Severity	Code	Description	Project	File	Line	Suppression State
  	         Warning	MVC1005	Using 'UseMvc' to configure MVC is not supported while using Endpoint Routing. 

    	     To continue using 'UseMvc', please set 'MvcOptions.EnableEndpointRouting = false' 
			 inside 'ConfigureServices'.	
			 EFCorePostgreSQL	C:\Users\boiko\Downloads\postgresql-ef-core-master\EFCorePostgreSQL\Startup.cs	49	Active

			 */
		}
	}
}
