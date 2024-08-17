using CoffeeShopAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CoffeeShopAPI.Services
{
	public static class DbManagementService
	{
		public static void MigrationInitialization(IApplicationBuilder app) 
		{
			using (var serviceScope = app.ApplicationServices.CreateScope())
			{
				serviceScope.ServiceProvider.GetService<CoffeeShopContext>().Database.Migrate();
			}
		}
	}
}
